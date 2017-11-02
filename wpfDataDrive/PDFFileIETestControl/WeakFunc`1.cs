using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareControl
{
	public class WeakFunc<TResult>
	{
		private Func<TResult> _staticFunc;

		protected System.Reflection.MethodInfo Method
		{
			get;
			set;
		}

		public bool IsStatic
		{
			get
			{
				return null != _staticFunc;
			}
		}

		public virtual string MethodName
		{
			get
			{
				return null != _staticFunc ?
					_staticFunc.Method.Name
					: Method.Name;
			}
		}

		protected WeakReference FuncReference
		{
			get;
			set;
		}

		protected WeakReference Reference
		{
			get;
			set;
		}

		public virtual bool IsAlive
		{
			get
			{
				if ( null == _staticFunc
					&& null == Reference )
				{
					return false;
				}

				if ( null != _staticFunc )
				{
					if ( null != Reference )
					{
						return Reference.IsAlive;
					}

					return true;
				}

				return Reference.IsAlive;
			}
		}

		public object Target
		{
			get
			{
				return null == Reference ? null : Reference.Target;
			}
		}

		protected object FuncTarget
		{
			get
			{
				return null == FuncReference ? null : FuncReference.Target;
			}
		}

		protected WeakFunc()
		{
		}

		public WeakFunc( Func<TResult> func )
			: this( func.Target, func )
		{
		}

		public WeakFunc( object target, Func<TResult> func )
		{
			if ( func.Method.IsStatic )
			{
				_staticFunc = func;

				if ( target != null )
				{
					Reference = new WeakReference( target );
				}

				return;
			}

			Method = func.Method;
			FuncReference = new WeakReference( func.Target );

			Reference = new WeakReference( target );
		}

		public TResult Execute()
		{
			if ( _staticFunc != null )
			{
				return _staticFunc.Invoke();
			}

			if ( IsAlive )
			{
				if ( null != Method
					&& null != FuncReference )
				{
					return (TResult)Method.Invoke( FuncTarget, null/*WeakFunc<TResult> 是无参数的*/ );
				}
			}

			return default( TResult );
		}

		public void MarkForDeletion()
		{
			Reference = null;
			FuncReference = null;
			Method = null;
			_staticFunc = null;
		}
	}
}
