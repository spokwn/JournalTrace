using JournalTrace.Entry;
using JournalTrace.Language;
using JournalTrace.Native;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JournalTrace.Entry
{
    public class EntryManager
    {
        #region events

        public event EventHandler<float> StatusProgressUpdate;
        public event EventHandler<bool> NextStatusUpdate;
        public event EventHandler WorkEnded;

        protected virtual void OnStatusProgressUpdate() =>
            StatusProgressUpdate?.Invoke(this, 1f);

        protected virtual void OnEntryAmountUpdate(bool completed) =>
            NextStatusUpdate?.Invoke(this, completed);

        protected virtual void OnWorkEnded() =>
            WorkEnded?.Invoke(this, null);

        #endregion events

        private DriveInfo selectedVolume;

        public void ChangeVolume(DriveInfo newVolume)
        {
            this.selectedVolume = newVolume;
        }

        public long SelectedUSN;
        public long OldestUSN;

        public Win32Api.USN_JOURNAL_DATA usnCurrentJournalState;
        private NtfsUsnJournal usnJournal = null;

        public IDictionary<long, USNEntry> USNEntries = new Dictionary<long, USNEntry>();
        public IDictionary<ulong, USNCollection> USNDirectories = new Dictionary<ulong, USNCollection>();
        public IDictionary<ulong, USNCollection> USNFiles = new Dictionary<ulong, USNCollection>();

        public ConcurrentDictionary<ulong, ResolvableIdentifier> parentFileReferenceIdentifiers = new ConcurrentDictionary<ulong, ResolvableIdentifier>();
        public int fileReferenceIndetifiersSize = 0;

        public void BeginScan()
        {
            parentFileReferenceIdentifiers.Clear();
            USNEntries.Clear();
            USNDirectories.Clear();
            USNFiles.Clear();

            usnCurrentJournalState = new Win32Api.USN_JOURNAL_DATA();

            try
            {
                usnJournal = new NtfsUsnJournal(selectedVolume);
                OnEntryAmountUpdate(true);
            }
            catch (Exception)
            {
                OnEntryAmountUpdate(false);
                return;
            }

            Win32Api.USN_JOURNAL_DATA journalState = new Win32Api.USN_JOURNAL_DATA();
            var rtn = usnJournal.GetUsnJournalState(ref journalState);
            if (rtn == NtfsUsnJournal.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
            {
                usnCurrentJournalState = journalState;
                OnEntryAmountUpdate(true);
            }
            else
            {
                OnEntryAmountUpdate(false);
                return;
            }

            uint reasonMask = Win32Api.USN_REASON_DATA_OVERWRITE |
                              Win32Api.USN_REASON_DATA_EXTEND |
                              Win32Api.USN_REASON_NAMED_DATA_OVERWRITE |
                              Win32Api.USN_REASON_NAMED_DATA_TRUNCATION |
                              Win32Api.USN_REASON_FILE_CREATE |
                              Win32Api.USN_REASON_FILE_DELETE |
                              Win32Api.USN_REASON_EA_CHANGE |
                              Win32Api.USN_REASON_SECURITY_CHANGE |
                              Win32Api.USN_REASON_RENAME_OLD_NAME |
                              Win32Api.USN_REASON_RENAME_NEW_NAME |
                              Win32Api.USN_REASON_INDEXABLE_CHANGE |
                              Win32Api.USN_REASON_BASIC_INFO_CHANGE |
                              Win32Api.USN_REASON_HARD_LINK_CHANGE |
                              Win32Api.USN_REASON_COMPRESSION_CHANGE |
                              Win32Api.USN_REASON_ENCRYPTION_CHANGE |
                              Win32Api.USN_REASON_OBJECT_ID_CHANGE |
                              Win32Api.USN_REASON_REPARSE_POINT_CHANGE |
                              Win32Api.USN_REASON_STREAM_CHANGE |
                              Win32Api.USN_REASON_CLOSE;

            OldestUSN = usnCurrentJournalState.FirstUsn;
            var rtnCode = usnJournal.GetUsnJournalEntries(usnCurrentJournalState, reasonMask, out List<Win32Api.UsnEntry> usnEntries, out usnCurrentJournalState);

            if (rtnCode == NtfsUsnJournal.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
            {
                OnEntryAmountUpdate(true);

                ResolveIdentifiers(usnEntries);
                OnEntryAmountUpdate(true);

                AddEntries(usnEntries);
                OnEntryAmountUpdate(true);

                OnWorkEnded();
            }
            else
            {
                OnEntryAmountUpdate(false);
            }
        }

        private void AddEntries(List<Win32Api.UsnEntry> usnEntries)
        {
            foreach (var entry in usnEntries)
            {
                ulong parentRef = entry.ParentFileReferenceNumber;
                ulong fileRef = entry.FileReferenceNumber;

                USNEntries.Add(entry.USN, new USNEntry(entry.USN, entry.Name, fileRef, parentRef, entry.TimeStamp, entry.Reason));

                if (!USNDirectories.TryGetValue(parentRef, out USNCollection foundDir))
                {
                    USNDirectories.Add(parentRef, new USNCollection(parentRef, entry.USN));
                }
                else
                {
                    foundDir.USNList.Add(entry.USN);
                }

                if (!USNFiles.TryGetValue(fileRef, out USNCollection foundFile))
                {
                    USNFiles.Add(fileRef, new USNCollection(fileRef, entry.USN));
                }
                else
                {
                    foundFile.USNList.Add(entry.USN);
                }
            }

            string usnReasonsRaw = LanguageManager.INSTANCE.GetString("usnreasons");
            string[] usnReasonsList = usnReasonsRaw.Split(new[] { ',' }, StringSplitOptions.None);

            Parallel.ForEach(USNEntries, entry =>
            {
                entry.Value.ResolveInfo(usnReasonsList);
            });
        }

        private void ResolveIdentifiers(List<Win32Api.UsnEntry> usnEntries)
        {
            HashSet<ulong> fileReference = new HashSet<ulong>();
            HashSet<ulong> parentReferences = new HashSet<ulong>();

            foreach (var entry in usnEntries)
            {
                fileReference.Add(entry.FileReferenceNumber);
                parentReferences.Add(entry.ParentFileReferenceNumber);
            }

            fileReferenceIndetifiersSize = fileReference.Count;

            Parallel.ForEach(parentReferences, id =>
            {
                parentFileReferenceIdentifiers.TryAdd(id, new ResolvableIdentifier(id));
            });

            Parallel.ForEach(parentFileReferenceIdentifiers, kvp =>
            {
                kvp.Value.Resolve();
            });
        }

        private TreeNode GetNodeOfName(TreeNode nodeToSearch, string name)
        {
            foreach (TreeNode node in nodeToSearch.Nodes)
            {
                if (node.Name.Equals(name))
                    return node;
            }
            return null;
        }

        public List<long> GetChangesOfDirectory(string path)
        {
            foreach (var usndir in USNDirectories)
            {
                if (parentFileReferenceIdentifiers[usndir.Key].ResolvedID.Equals(path))
                {
                    return usndir.Value.USNList;
                }
            }
            return null;
        }

        public TreeNode[] BakeTree()
        {
            var listOfSplitDirectories = new ConcurrentBag<string[]>();

            Parallel.ForEach(USNDirectories, usndir =>
            {
                string resolvedID = parentFileReferenceIdentifiers[usndir.Key].ResolvedID;
                string[] directories = resolvedID.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                listOfSplitDirectories.Add(directories);
            });

            TreeNodeCollection treeNodes = PopulateTreeView(listOfSplitDirectories, '\\');
            return treeNodes.Cast<TreeNode>().ToArray();
        }

        private TreeNodeCollection PopulateTreeView(ConcurrentBag<string[]> listOfPaths, char pathSeparator)
        {
            TreeNode root = new TreeNode();
            foreach (string[] pathParts in listOfPaths.Where(p => p != null))
            {
                TreeNode currentNode = root;
                foreach (string part in pathParts)
                {
                    var existingNode = currentNode.Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Text.Equals(part));
                    if (existingNode == null)
                    {
                        existingNode = currentNode.Nodes.Add(part);
                        existingNode.Name = part;
                        existingNode.Text = part;
                        existingNode.ForeColor = Color.Gray;
                    }
                    currentNode = existingNode;
                }
            }
            return root.Nodes;
        }
    }
}
