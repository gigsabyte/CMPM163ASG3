using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    Animator anim;
	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		// this rotates each cube.
		//transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);


		// --------------------------------------------------------
		// ------- animate the cube size based on spectrum data.

		// consolidate spectral data to 8 partitions (1 partition for each rotating cube)
		int numPartitions = 8;
		float[] aveMag = new float[numPartitions];
		float partitionIndx = 0;
		int numDisplayedBins = 512 / 2; //NOTE: we only display half the spectral data because the max displayable frequency is Nyquist (at half the num of bins)

		for (int i = 0; i < numDisplayedBins; i++) 
		{
			if(i < numDisplayedBins * (partitionIndx + 1) / numPartitions){
				aveMag[(int)partitionIndx] += AudioPeer.spectrumData [i] / (512/numPartitions);
			}
			else{
				partitionIndx++;
				i--;
			}
		}

		// scale and bound the average magnitude.
		for(int i = 0; i < numPartitions; i++)
		{
			aveMag[i] = (float)0.5 + aveMag[i]*100;
			if (aveMag[i] > 100) {
				aveMag[i] = 100;
			}
		}

		// Map the magnitude to the cubes based on the cube name.
		if (gameObject.name == "deer_1") 
		{
            anim.SetFloat("partySpeed", aveMag[0]);
		}

		// --------- End animating cube via spectral data
		// --------------------------------------------------------



	}


}

