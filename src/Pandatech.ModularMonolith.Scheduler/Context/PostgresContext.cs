using Microsoft.EntityFrameworkCore;

namespace Pandatech.ModularMonolith.Scheduler.Context;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options);
