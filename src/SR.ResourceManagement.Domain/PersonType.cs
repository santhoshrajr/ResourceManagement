using System;
using System.Collections.Generic;

namespace SR.ResourceManagement.Domain 
{
    public class PersonType 
    {
       public int Id { get; set; }

       public string Name { get; set; }
       public ICollection<PersonPersonType> PersonPersonType { get; set; }
    }
}