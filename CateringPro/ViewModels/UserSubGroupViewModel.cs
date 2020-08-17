using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringPro.Models;
namespace CateringPro.ViewModels
{
    public class Nodes: Dictionary<int, UserSubGroupNode>
    {
        public List<UserSubGroupNode> AsList()
        {
            return this.Values.ToList();
        }
    }
    public class UserSubGroupViewModel
    {
        private Nodes nodes = new Nodes();
        private Nodes root = new Nodes();
        public Nodes AllNodes => nodes;
        public Nodes Root => root;

        public void  BuildFrom(IEnumerable<UserSubGroup> src)
        {
            AllNodes.Clear();
            using (var sequenceEnum = src.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    AllNodes.Add(sequenceEnum.Current.Id,new UserSubGroupNode( sequenceEnum.Current));
                }
            }
            AllNodes.Values.ToList().ForEach(n => n.BuildNode(this));

        }
    }

    public class UserSubGroupNode
    {
        private Nodes childs =new Nodes();

        private UserSubGroupNode parent=null;
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public UserSubGroupNode(UserSubGroup subgroup)
        {
            Id = subgroup.Id;
            Name = subgroup.Name;
            ParentId = subgroup.ParentId;

        }
        private void AttachParent(UserSubGroupNode _parent)
        {
            parent = _parent;
        }
        private void RegisterChild(UserSubGroupNode child)
        {
            Childs[child.Id] = child;
        }
        public void BuildNode(UserSubGroupViewModel tree)
        {
            if (!this.ParentId.HasValue) //root
            {
                tree.Root[Id] = this;
            }
            else
            {
                if (tree.AllNodes.ContainsKey(this.ParentId.Value))
                {
                    UserSubGroupNode parentnode = tree.AllNodes[this.ParentId.Value];
                    AttachParent(parentnode);
                    parentnode.RegisterChild(this);
                }
            }
        }
        public Nodes Childs => childs;
        public UserSubGroupNode ParentNode => parent;

        public List<UserSubGroupNode> ChildList
        {
            get
            {
                return Childs.Values.ToList();
            }
        }


    }
}
