﻿using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.DTO.Images
{
    public class ImageDTO
    {
        public ImageDTO()
        {
            Trip = new();
        }

        public int ImageId { get; set; }

        public string Link { get; set; }

        public int TripId { get; set; }

        public TripDTO Trip { get; set; }
    }
}