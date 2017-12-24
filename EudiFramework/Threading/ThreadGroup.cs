using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EudiFramework.Threading
{
    public class ThreadGroup : IDisposable
    {
        private static List<ThreadGroup> m_Groups = new List<ThreadGroup>();

        public Dictionary<int, WorkerTask> Tasks = new Dictionary<int, WorkerTask>();
        public Type MetaCreationType;
        public int MetaCreationGroupId;

        public ThreadGroup(Type type, int groupId)
        {
            MetaCreationType = type;
            MetaCreationGroupId = groupId;

            m_Groups.Add(this);
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            NormalDispose();
            m_Groups.Remove(this);
        }

        internal void NormalDispose()
        {
            foreach (var task in Tasks)
            {
                task.Value.Dispose();
            }

            Tasks.Clear();
            Tasks = null;

            GC.SuppressFinalize(this);
        }

        internal void DisposeByQuitEvent()
        {
            NormalDispose();
        }

        internal static void DestroyEverything()
        {
            var groupsCount = m_Groups.Count;
            for (var i = 0; i < groupsCount; i++)
            {
                var group = m_Groups[i];
                group.DisposeByQuitEvent();
            }

            m_Groups.Clear();
            m_Groups = null;
        }
    }
}
