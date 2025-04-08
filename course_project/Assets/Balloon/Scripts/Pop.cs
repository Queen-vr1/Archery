using UnityEngine;

public class PopBalloon : MonoBehaviour
{
    public ParticleSystem particleFx;
    public AudioSource audioSource;
    public Renderer rend;
    public Texture2D newTexture;
    public Texture2D newTexture2;
    public Texture2D newTexture3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayPop()
    {
    	if (particleFx != null){ 
	    	particleFx.Stop();    
	    	particleFx.Clear();    
	   		particleFx.Play(); ; 
	 	}
		
		if (audioSource != null){
			audioSource.Stop();
			audioSource.Play();
		}
    }

    public void ChangeImg()
    {
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }

        if (rend != null && rend.material != null)
        {
            Texture actualTexure = rend.material.mainTexture;

            if (actualTexure != null)
            {
                if (newTexture != null)
                {
                    rend.material.mainTexture = newTexture;
                }
            }
        }
    }


    public void ChangeImg2()
    {
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }

        if (rend != null && rend.material != null)
        {
            Texture actualTexure = rend.material.mainTexture;

            if (actualTexure != null)
            {
                if (newTexture2 != null)
                {
                    rend.material.mainTexture = newTexture2;
                }
            }
        }
    }


    public void ChangeImg3()
    {
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }

        if (rend != null && rend.material != null)
        {
            Texture actualTexure = rend.material.mainTexture;

            if (actualTexure != null)
            {
                if (newTexture3 != null)
                {
                    rend.material.mainTexture = newTexture3;
                }
            }
        }
    }
   
}
