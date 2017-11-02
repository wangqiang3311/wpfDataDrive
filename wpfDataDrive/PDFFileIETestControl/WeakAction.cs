using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareControl
{
	public class WeakAction
	{
		private Action _staticAction;

		protected System.Reflection.MethodInfo Method
		{
			get;
			set;
		}

		/// <summary>
		/// 获取和设置 Target目标的弱引用.
		/// </summary>
		protected WeakReference Reference
		{
			get;
			set;
		}

		/// <summary>
		/// 获取和设置 Action目标的弱引用.
		/// </summary>
		protected WeakReference ActionReference
		{
			get;
			set;
		}

		public bool IsStatic
		{
			get { return _staticAction != null; }
		}

		public virtual string MethodName
		{
			get
			{
				if ( _staticAction != null )
				{
					return _staticAction.Method.Name;
				}

				return Method.Name;
			}
		}

		/// <summary>
		/// 获取当前 System.WeakReference 对象引用的对象是否已被垃圾回收的指示。活着:True,回收了:false
		/// </summary>
		public virtual bool IsAlive
		{
			get
			{
				if ( _staticAction == null
					&& Reference == null )
				{
					return false;
				}

				if ( _staticAction != null )
				{
					if ( Reference != null )
					{
						return Reference.IsAlive;
					}

					return true;
				}

				return Reference.IsAlive;
			}
		}

		/// <summary>
		/// 获取构造时的Target
		/// target参数
		/// </summary>
		public object Target
		{
			get
			{
				if ( Reference == null )
				{
					return null;
				}

				return Reference.Target;
			}
		}

		/// <summary>
		/// Action的目标.
		/// 构造时Action参数的Target,非参数target
		/// </summary>
		protected object ActionTarget
		{
			get
			{
				if ( ActionReference == null )
				{
					return null;
				}

				return ActionReference.Target;
			}
		}

		/// <summary>
		/// 动作执行.
		/// 只有IsAlive=true时才有效.
		/// </summary>
		public void Execute()
		{
			if ( _staticAction != null )
			{
				_staticAction();
				return;
			}

			var actionTarget = ActionTarget;

			if ( IsAlive )
			{
				if ( Method != null
					&& ActionReference != null
					&& actionTarget != null )
				{
					Method.Invoke( ActionTarget, null );
					return;
				}
			}
		}

		/// <summary>
		/// 清理重置.
		/// </summary>
		public void MarkForDeletion()
		{
			Reference = null;
			ActionReference = null;
			Method = null;
			_staticAction = null;
		}

		protected WeakAction()
		{
		}

		public WeakAction( Action action )
			: this( action.Target, action )
		{

		}

		public WeakAction( object target, Action action )
		{
			if ( action.Method.IsStatic )
			{
				_staticAction = action;

				if ( target != null )
				{
					// Keep a reference to the target to control the
					// WeakAction's lifetime.
					Reference = new WeakReference( target );
				}

				return;
			}

			Method = action.Method;
			ActionReference = new WeakReference( action.Target );

			Reference = new WeakReference( target );
		}
	}
}
