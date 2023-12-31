﻿using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public partial class Music
    {
        public Music() { }

        public Music(Guid id, string name, long fileSize, User user, Cover? cover)
        {
            Id = id;
            Name = name;
            FileSize = fileSize;
            User = user;
            UploadDate = DateTime.Now;
            Cover = cover;
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 3)]
        public string? Name { get; set; }

        [Required]
        public long FileSize { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        [Required]
        public User? User { get; set; }

        public Cover? Cover { get; set; }

    }
}
