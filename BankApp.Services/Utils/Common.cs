using BankApp.Services.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Services.Utils;

public static class Common
{
    public static string ActionTypeToString(ActionType type)
    {
        switch (type)
        {
            case ActionType.Deposit:
                return "Deposit";
            case ActionType.Withdraw:
                return "Withdraw";
            case ActionType.Transfer:
                return "Transfer";
            default:
                throw new InvalidEnumArgumentException($"{nameof(type)} is unsupported");
        }
    }

    public static ActionType ActionTypeStringToEnum(string type)
    {
        switch (type.ToLower())
        {
            case "deposit":
                return ActionType.Deposit;
            case "withdraw":
                return ActionType.Withdraw;
            case "transfer":
                return ActionType.Transfer;
            default:
                throw new Exception($"{type} is unsupported");
        }
    }
    public static string TransactionTypeToString(TransactionType type)
    {
        switch (type)
        {
            case TransactionType.Debit:
                return "Debit";
            case TransactionType.Credit:
                return "Credit";
            default:
                throw new InvalidEnumArgumentException($"{nameof(type)} is unsupported");
        }
    }

    public static TransactionType TransactionTypeStringToEnum(string type)
    {
        switch (type.ToLower())
        {
            case "credit":
                return TransactionType.Credit;
            case "debit":
                return TransactionType.Debit;
            default:
                throw new Exception($"{type} is unsupported");
        }
    }
}
