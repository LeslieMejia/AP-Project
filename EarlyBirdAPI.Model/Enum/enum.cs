using EarlyBirdAPI;

public enum UserRole
{
    Employer,  // Matches 'employer' in the database
    JobSeeker  // Matches 'jobseeker' in the database
}

public enum JobPostingStatus
{
    Open,      // Matches 'open' in the database
    Closed     // Matches 'closed' in the database
}

public enum ApplicationStatus
{
    UnderReview,  // Matches 'under review' in the database
    Interview,    // Matches 'interview' in the database
    Hired,        // Matches 'hired' in the database
    Rejected      // Matches 'rejected' in the database
}
