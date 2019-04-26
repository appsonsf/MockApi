using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MdmDataDistributeApi.ViewModels
{
    public class ContactInfo
    {
        public Guid Id { get; set; }

        public string SrcId { get; set; }

        public Guid? UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string IdCardNo { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        public string Number { get; set; }

        public int Gender { get; set; }

        public Guid DepartmentId { get; set; }

        public string DepartmentSrcId { get; set; }
    }
}
