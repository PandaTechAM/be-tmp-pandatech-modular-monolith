namespace Pandatech.ModularMonolith.SharedKernel.Interfaces;

public interface IRequestContext //todo this is for auth module
{
   //public Identity Identity { get; set; }
   //public MetaData MetaData { get; set; }
   public bool IsAuthenticated { get; set; }
}