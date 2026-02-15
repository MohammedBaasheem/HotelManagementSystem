namespace Domain.Enums
{
    public enum Feature
    {
        // =========================
        // Role Management (100 - 199)
        // =========================

        GetAllRoles = 100,
        CreateRole = 101,
        DeleteRole = 102,

        AssignFeatureToRole = 110,
        UnassignFeatureFromRole = 111,


        // =========================
        // User Authentication (200 - 299)
        // =========================

        RegisterUser = 200,
        LoginUser = 201,
        ViewOwnProfile = 202,


        // =========================
        // User Management (300 - 399)
        // =========================

        DeleteUser = 300,
        AssignRoleToUser = 301,
        UnassignRoleFromUser = 302,
        CreateStaffUser = 303,


        // =========================
        // Offer Management (600 - 699)
        // =========================
        CreateOffer = 600,
        UpdateOffer=601,
        DeleteOffer = 602,
        GetOfferById = 603,
        GetAllOffers = 604,
        GetFilteredOffers=605,


    }
}
