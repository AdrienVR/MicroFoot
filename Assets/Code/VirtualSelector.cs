using UnityEngine;

public abstract class VirtualSelector : MonoBehaviour 
{
	public abstract void Select();
	
	public abstract void Deselect();

	public virtual void UpAction()
	{
		
	}
	
	public virtual void DownAction()
	{
		
	}
}
