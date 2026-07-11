using UnityEngine;

public class ManequinBehaviour : MonoBehaviour
{
	public Rigidbody manequin;

	public GameObject[] items;

	private bool used;

	public void OnAction()
	{
		manequin.transform.parent = null;
		manequin.isKinematic = false;
		Invoke("Message", 1f);
	}

	public void Message()
	{
		MessagesManager.instance.ShowMessage("mannequin_drop");
	}

	public void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Floor" && !used)
		{
			used = true;
			GameObject[] array = items;
			foreach (GameObject gameObject in array)
			{
				gameObject.transform.parent = null;
				gameObject.GetComponent<Collider>().enabled = true;
				gameObject.GetComponent<Rigidbody>().isKinematic = false;
				gameObject.GetComponent<Rigidbody>().AddExplosionForce(2f, base.transform.position, 0.5f);
			}
		}
	}

	[ContextMenu("FallManually")]
	public void FallManually()
	{
		manequin.transform.parent = null;
		manequin.isKinematic = false;
		Invoke("Message", 1f);
	}
}
