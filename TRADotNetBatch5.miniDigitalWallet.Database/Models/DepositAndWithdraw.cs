using System;
using System.Collections.Generic;

namespace TRADotNetBatch5.miniDigitalWallet.Database.Models;

public partial class DepositAndWithdraw
{
    public int DepositId { get; set; }

    public string MobileNumber { get; set; } = null!;

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = null!;

    public virtual WalletUser MobileNumberNavigation { get; set; } = null!;
}
