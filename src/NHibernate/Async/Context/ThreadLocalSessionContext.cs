﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Engine;

namespace NHibernate.Context
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class ThreadLocalSessionContext : ICurrentSessionContext
	{

		private static async Task CleanupAnyOrphanedSessionAsync(ISessionFactory factory, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			ISession orphan = DoUnbind(factory, false);

			if (orphan != null)
			{
				log.Warn("Already session bound on call to bind(); make sure you clean up your sessions!");

				try
				{
					if (orphan.Transaction != null && orphan.Transaction.IsActive)
					{
						try
						{
							await (orphan.Transaction.RollbackAsync(cancellationToken)).ConfigureAwait(false);
						}
						catch (Exception ex)
						{
							log.Debug("Unable to rollback transaction for orphaned session", ex);
						}
					}
					orphan.Close();
				}
				catch (Exception ex)
				{
					log.Debug("Unable to close orphaned session", ex);
				}
			}
		}

		public static async Task BindAsync(ISession session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			ISessionFactory factory = session.SessionFactory;
			await (CleanupAnyOrphanedSessionAsync(factory, cancellationToken)).ConfigureAwait(false);
			DoBind(session, factory);
		}
	}
}