namespace ElsaServer.Workflows
{
    public enum BankGuaranteeStandardState
    {
        None = 1,

        Created = 5,
        
        ApprovedByExpert = 10,
        RejectedByExpert = 12,
        
        ApprovedByPM = 15,
        RejectedByPm = 17
    }

}
