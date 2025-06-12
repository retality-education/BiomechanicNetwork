using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomechanicNetwork.Models
{
    internal class Comment
    {
        public int Id { get; set; }
        public int VideoPostId { get; set; }
        public int UserId { get; set; }
        public bool IsExpert {  get; set; }
        public string Text { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
