using System;
using System.Collections.Generic;

namespace TripInfoREST.API.Entities
{
    public static class TripContextExtensions
    {
        public static void EnsureSeedDataForContext(this TripContext context)
        {
            // first, clear the database.  This ensures we can always start 
            // fresh with each demo.  Not advised for production environments, obviously :-)

            context.Destinations.RemoveRange(context.Destinations);
            context.SaveChanges();

            // init seed data
            var destinations = new List<Destination>()
            {
                new Destination()
                {
                     Id = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf"),
                     Name = "Las Vegas",
                     State = "Nevada",
                     Genre = "City",
                     DateOfVisit = new DateTimeOffset(new DateTime(2016, 3, 24)),
                     Attractions = new List<Attraction>()
                     {
                         new Attraction()
                         {
                             Id = new Guid("c7ba6add-09c4-45f8-8dd0-eaca221e5d93"),
                             Name = "Bellagio",
                             Description = "Bellagio is a resort, luxury hotel and casino on the Las Vegas Strip in Paradise, Nevada."
                         },
                         new Attraction()
                         {
                             Id = new Guid("a3749477-f823-4124-aa4a-fc9ad5e79cd6"),
                             Name = "The Venetian",
                             Description = "The Venetian Resort Hotel Casino is a five-diamond luxury hotel and casino resort located on the Las Vegas Strip in Paradise, Nevada."
                         },
                         new Attraction()
                         {
                             Id = new Guid("70a1f9b9-0a37-4c1a-99b1-c7709fc64167"),
                             Name = "Stratosphere",
                             Description = "The Stratosphere Las Vegas is a hotel, casino, and tower located on Las Vegas Boulevard just north of the Las Vegas Strip in Las Vegas, Nevada."
                         },
                         new Attraction()
                         {
                             Id = new Guid("60188a2b-2784-4fc4-8df8-8919ff838b0b"),
                             Name = "The Mirage",
                            Description = "The Mirage is a 3,044 room Polynesian-themed resort and casino resort located on the Las Vegas Strip in Paradise, Nevada."
                         }
                     }
                },
                new Destination()
                {
                     Id = new Guid("76053df4-6687-4353-8937-b45556748abe"),
                     Name = "Lake Mead National Recreation Area",
                     State = "Nevada",
                     Genre = "Recreation",
                     DateOfVisit = new DateTimeOffset(new DateTime(2016, 3, 25)),
                     Attractions = new List<Attraction>()
                     {
                         new Attraction()
                         {
                             Id = new Guid("447eb762-95e9-4c31-95e1-b20053fbe215"),
                             Name = "Redstone",
                             Description = "The forces that created this landscape are revealed in the exposed rainbow-colored layers of rock."
                         },
                         new Attraction()
                         {
                             Id = new Guid("bc4c35c3-3857-4250-9449-155fcf5109ec"),
                             Name = "Lake Mead",
                             Description = "With striking landscapes and brilliant blue waters, this year-round playground spreads across 1.5 million acres of mountains, canyons, valleys and two vast lakes."
                         }
                     }
                },
                new Destination()
                {
                     Id = new Guid("412c3012-d891-4f5e-9613-ff7aa63e6bb3"),
                     Name = "Hoover Dam",
                     State = "Nevada",
                     Genre = "Man made Wonders",
                     DateOfVisit = new DateTimeOffset(new DateTime(2016, 3, 25)),
                     Attractions = new List<Attraction>()
                     {
                         new Attraction()
                         {
                             Id = new Guid("9edf91ee-ab77-4521-a402-5f188bc0c577"),
                             Name = "Observation Deck",
                            Description = "Hoover Dam is a concrete arch-gravity dam in the Black Canyon of the Colorado River, on the border between the U.S. states of Nevada and Arizona."
                         }
                     }
                },
                new Destination()
                {
                     Id = new Guid("578359b7-1967-41d6-8b87-64ab7605587e"),
                     Name = "San Francisco",
                     State = "California",
                     Genre = "City",
                     DateOfVisit = new DateTimeOffset(new DateTime(2016, 11, 23)),
                     Attractions = new List<Attraction>()
                     {
                         new Attraction()
                         {
                             Id = new Guid("01457142-358f-495f-aafa-fb23de3d67e9"),
                             Name = "Chinatown",
                            Description = "This is one of the oldest and most established Chinatowns in the U.S. Beyond iconic Dragon’s Gate, a bustling maze of streets and alleys brims with dim sum joints and other traditional eateries."
                         }
                     }
                },
                new Destination()
                {
                     Id = new Guid("f74d6899-9ed2-4137-9876-66b070553f8f"),
                     Name = "Yosemite National Park",
                     State = "California",
                     Genre = "U.S. National Parks",
                     DateOfVisit = new DateTimeOffset(new DateTime(2017, 4, 30)),
                     Attractions = new List<Attraction>()
                     {
                         new Attraction()
                         {
                             Id = new Guid("e57b605f-8b3c-4089-b672-6ce9e6d6c23f"),
                             Name = "Vernal Falls",
                            Description = "Vernal Fall is a 317-foot waterfall on the Merced River just downstream of Nevada Fall in Yosemite National Park, California."
                         },
                         new Attraction()
                         {
                             Id = new Guid("09af5a52-9421-44e8-a2bb-a6b9ccbc8239"),
                             Name = "Mirror Lake",
                            Description = "Mirror Lake is a small, seasonal lake located on Tenaya Creek in Yosemite National Park, California."
                         }
                     }
                },
                new Destination()
                {
                     Id = new Guid("a1da1d8e-1988-4634-b538-a01709477b77"),
                     Name = "Sequoia National Park",
                     State = "California",
                     Genre = "U.S. National Parks",
                     DateOfVisit = new DateTimeOffset(new DateTime(2017, 5, 24)),
                     Attractions = new List<Attraction>()
                     {
                         new Attraction()
                         {
                             Id = new Guid("1325360c-8253-473a-a20f-55c269c20407"),
                             Name = "Moro Rock",
                             Description = "Moro Rock is a granite dome rock formation in Sequoia National Park, California."
                         }
                     }
                }
            };

            //var tokenRequests = new List<TokenRequest>()
            //{
            //    new TokenRequest()
            //    {
            //        Id = new Guid("0647a8e4-64dd-4f31-81be-90ddc8cf71d4"),
            //        Username = "Ruchi",
            //        Password = "password"
            //    },
            //    new TokenRequest()
            //    {
            //        Id = new Guid("805a7306-3e56-4a26-9772-b0037dcbc14b"),
            //        Username = "Swapnil",
            //        Password = "password"
            //    }
            //};

            //context.TokenRequests.AddRange(tokenRequests);
            context.Destinations.AddRange(destinations);
            context.SaveChanges();
        }
    }
}