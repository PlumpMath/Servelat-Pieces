﻿using System;
using System.Linq.Expressions;
using System.Threading;

namespace Xtalion.Async
{
	public class AsyncQuery<TResult> : AsyncCall<TResult>
	{
		readonly Func<TResult> call;

		public AsyncQuery(Expression<Func<TResult>> expression)
		{
			call = expression.Compile();
		}

		public override void Execute()
		{
			ThreadPool.QueueUserWorkItem(ExecuteCall, null);
		}

		void ExecuteCall(object state)
		{
			try
			{
				Result = call();
			}
			catch (Exception exc)
			{
				Exception = exc;
			}

			SignalCompleted();
		}
	}
}
