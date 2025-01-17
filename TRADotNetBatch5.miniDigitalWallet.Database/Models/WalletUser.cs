using System;
using System.Collections.Generic;

namespace TRADotNetBatch5.miniDigitalWallet.Database.Models;

public partial class WalletUser
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string MobileNumber { get; set; } = null!;

    public string PinCode { get; set; } = null!;

    public decimal? Balance { get; set; }

    public virtual ICollection<DepositAndWithdraw> DepositAndWithdraws { get; set; } = new List<DepositAndWithdraw>();

    public virtual ICollection<Transaction> TransactionReceiverMobileNoNavigations { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionSenderMobileNoNavigations { get; set; } = new List<Transaction>();
}           
