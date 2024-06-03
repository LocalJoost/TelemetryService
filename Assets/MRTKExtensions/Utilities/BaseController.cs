using UnityEngine;
namespace MRTKExtensions.Utilities
{
    public abstract class BaseController : MonoBehaviour
    {
        [SerializeField]
        protected GameObject visuals;

        protected virtual void Awake()
        {
            if (visuals == null && transform.childCount > 0) 
            {
                visuals = transform.GetChild(0).gameObject;
            }
        }
    }
}
