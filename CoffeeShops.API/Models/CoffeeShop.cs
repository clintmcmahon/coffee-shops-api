using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShops.API.Models;

[Table("CoffeeShops")]
public class CoffeeShop
{
    [Key]
    [Column("placeId")]
    public string PlaceId { get; set; } = string.Empty;

    [Column("name")]
    public string? Name { get; set; }

    [Column("nameForEmails")]
    public string? NameForEmails { get; set; }

    [Column("site")]
    public string? Site { get; set; }

    [Column("subtypes")]
    public string? Subtypes { get; set; }

    [Column("category")]
    public string? Category { get; set; }

    [Column("type")]
    public string? Type { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("fullAddress")]
    public string? FullAddress { get; set; }

    [Column("borough")]
    public string? Borough { get; set; }

    [Column("street")]
    public string? Street { get; set; }

    [Column("city")]
    public string? City { get; set; }

    [Column("postalCode")]
    public string? PostalCode { get; set; }

    [Column("state")]
    public string? State { get; set; }

    [Column("usState")]
    public string? UsState { get; set; }

    [Column("country")]
    public string? Country { get; set; }

    [Column("countryCode")]
    public string? CountryCode { get; set; }

    [Column("latitude")]
    public double? Latitude { get; set; }

    [Column("longitude")]
    public double? Longitude { get; set; }

    [Column("h3")]
    public string? H3 { get; set; }

    [Column("timeZone")]
    public string? TimeZone { get; set; }

    [Column("plusCode")]
    public string? PlusCode { get; set; }

    [Column("areaService")]
    public string? AreaService { get; set; }

    [Column("rating")]
    public double? Rating { get; set; }

    [Column("reviews")]
    public string? Reviews { get; set; }

    [Column("reviewsLink")]
    public string? ReviewsLink { get; set; }

    [Column("reviewsTags")]
    public string? ReviewsTags { get; set; }

    [Column("reviewsPerScore")]
    public string? ReviewsPerScore { get; set; }

    [Column("reviewsPerScore1")]
    public int? ReviewsPerScore1 { get; set; }

    [Column("reviewsPerScore2")]
    public int? ReviewsPerScore2 { get; set; }

    [Column("reviewsPerScore3")]
    public int? ReviewsPerScore3 { get; set; }

    [Column("reviewsPerScore4")]
    public int? ReviewsPerScore4 { get; set; }

    [Column("reviewsPerScore5")]
    public int? ReviewsPerScore5 { get; set; }

    [Column("photosCount")]
    public int? PhotosCount { get; set; }

    [Column("photo")]
    public string? Photo { get; set; }

    [Column("streetView")]
    public string? StreetView { get; set; }

    [Column("locatedIn")]
    public string? LocatedIn { get; set; }

    [Column("workingHours")]
    public string? WorkingHours { get; set; }

    [Column("workingHoursOldFormat")]
    public string? WorkingHoursOldFormat { get; set; }

    [Column("otherHours")]
    public string? OtherHours { get; set; }

    [Column("popularTimes")]
    public string? PopularTimes { get; set; }

    [Column("businessStatus")]
    public string? BusinessStatus { get; set; }

    [Column("about")]
    public string? About { get; set; }

    [Column("range")]
    public string? Range { get; set; }

    [Column("posts")]
    public string? Posts { get; set; }

    [Column("logo")]
    public string? Logo { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("typicalTimeSpent")]
    public string? TypicalTimeSpent { get; set; }

    [Column("verified")]
    public bool? Verified { get; set; }

    [Column("ownerId")]
    public string? OwnerId { get; set; }

    [Column("ownerTitle")]
    public string? OwnerTitle { get; set; }

    [Column("ownerLink")]
    public string? OwnerLink { get; set; }

    [Column("reservationLinks")]
    public string? ReservationLinks { get; set; }

    [Column("bookingAppointmentLink")]
    public string? BookingAppointmentLink { get; set; }

    [Column("menuLink")]
    public string? MenuLink { get; set; }

    [Column("orderLinks")]
    public string? OrderLinks { get; set; }

    [Column("locationLink")]
    public string? LocationLink { get; set; }

    [Column("locationReviewsLink")]
    public string? LocationReviewsLink { get; set; }

    [Column("googleId")]
    public string? GoogleId { get; set; }

    [Column("cid")]
    public string? Cid { get; set; }

    [Column("kgmid")]
    public string? Kgmid { get; set; }

    [Column("reviewsId")]
    public string? ReviewsId { get; set; }

    [Column("locatedGoogleId")]
    public string? LocatedGoogleId { get; set; }

    [Column("lastUpdated")]
    public DateTime? LastUpdated { get; set; }

    [Column("aiOverview")]
    public string? AiOverview { get; set; }

    [Column("aiDescription")]
    public string? AiDescription { get; set; }

    [Column("isDeleted")]
    public bool? IsDeleted { get; set; }

    [Column("aiReview")]
    public string? AiReview { get; set; }

    [Column("isChain")]
    public bool? IsChain { get; set; }

        [Column("workFromMSPRating")]
    public string? WorkFromMSPRating { get; set; }

        [Column("workFromMSPInstagram")]
    public string? WorkFromMSPInstagram { get; set; }
    // AI Classification fields
        public decimal? ClassificationConfidence { get; set; }
        public string? ClassificationReasoning { get; set; }
        public string? SpecialtyStatus { get; set; }  // auto_approved, auto_rejected, pending, approved, rejected
        public decimal? ContentConfidence { get; set; }
        public string? WhyWeLoveIt { get; set; }
        public string? WhatToOrder { get; set; }
        public string? Vibe { get; set; }
        public string? GoodFor { get; set; }  // JSON array stored as string
        public DateTime? ClassifiedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
}
