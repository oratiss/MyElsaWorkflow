namespace ElsaServer.Workflows
{
    public enum BankGuaranteeState
    {
        None = 1,

        Created = 5,

        ApprovedByExpert = 10,
        RejectedByExpert = 12,

        ApprovedByPM = 15,
        RejectedByPm = 17
    }

}
