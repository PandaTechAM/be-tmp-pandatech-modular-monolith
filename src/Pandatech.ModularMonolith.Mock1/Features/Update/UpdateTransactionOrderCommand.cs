﻿using Pandatech.ModularMonolith.SharedKernel.Interfaces;

namespace Pandatech.ModularMonolith.Mock1.Features.Update;

public class UpdateTransactionOrderCommand : ICommand
{
   public long Id { get; set; }
}