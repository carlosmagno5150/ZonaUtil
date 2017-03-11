using System;

namespace GpSystem.Domain.Validation
{
	public class Clicker
	{		
		private readonly Func<bool> _handleTry;
		private readonly Action<Exception> _handleException;

		public Clicker(Action<Exception> handleException, Func<bool> handleTry)
		{
			_handleException = handleException;
			_handleTry = handleTry;			
		}

		public ClickerResult Click()
		{
			try
			{
				return _handleTry.Invoke() ? ClickerResult.Sucesso : ClickerResult.NaoTerminado;
			}
			catch (Exception ex)
			{
				_handleException?.Invoke(ex);				
			}
			return ClickerResult.Erro;
		}

	}
}