using System;
using System.Collections.Generic;

namespace TRADotNetBatch5.miniDigitalWallet.Database.Models;

public partial class Transaction
{
    public int TransferId { get; set; }

    public string SenderMobileNo { get; set; } = null!;

    public string ReceiverMobileNo { get; set; } = null!;

    public decimal? Amount { get; set; }

    public string? Notes { get; set; }

    public virtual WalletUser ReceiverMobileNoNavigation { get; set; } = null!;

    public virtual WalletUser SenderMobileNoNavigation { get; set; } = null!;
}
