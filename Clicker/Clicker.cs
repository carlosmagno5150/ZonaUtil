using System;

namespace ObserverPattern.Clicker
{
	public class Clicker
	{		
		private readonly Func<bool> _handleTry;
		private readonly Action<Exception> _handleException;

		private Action _onSuccess;
		private Action _onFail;

		public Clicker SetOnSuccess(Action onSuccess)
		{
			_onSuccess = onSuccess;
			return this;
		}

		public Clicker SetOnFail(Action onFail)
		{
			_onFail = onFail;
			return this;			
		}

		public Clicker(Func<bool> handleTry, Action<Exception> handleException = null)
		{
			_handleException = handleException;
			_handleTry = handleTry;			
		}

		public ClickerResult Click()
		{
			try
			{
				if (_handleTry.Invoke())
				{
					_onSuccess?.Invoke();
					return ClickerResult.Sucesso;
				}
				else
				{
					_onFail?.Invoke();
					return ClickerResult.NaoTerminado;
				}
			}
			catch (Exception ex)
			{
				_handleException?.Invoke(ex);				
			}
			return ClickerResult.Erro;
		}

	}
}