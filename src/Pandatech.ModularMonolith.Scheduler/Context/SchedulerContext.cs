using Microsoft.EntityFrameworkCore;

namespace Pandatech.ModularMonolith.Scheduler.Context;

public class SchedulerContext(DbContextOptions<SchedulerContext> options) : DbContext(options);