using System;
using System.Collections.Generic;

namespace WebAPi.Models
{
    public partial class Author
    {
        public Author()
        {
            BookAuthor = new HashSet<BookAuthor>();
        }

        public int AuthorId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string EmailAddress { get; set; }

        public virtual ICollection<BookAuthor> BookAuthor { get; set; }
    }
}
