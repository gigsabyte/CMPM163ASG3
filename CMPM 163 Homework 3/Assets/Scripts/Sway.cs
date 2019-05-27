using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour {

    Renderer rend;
    float dir;
    float[] avgs;
    float sum;
    int index;

	// Use this for initialization
	void Start () {
        avgs = new float[8];
        for(int i = 0; i < avgs.Length; ++i)
        {
            avgs[i] = 0.5f;
            sum += avgs[i];
        }
        index = 0;
        rend = gameObject.GetComponent<Renderer>();
        dir = 1;
	}
	
	// Update is called once per frame
	void Update () {

		// consolidate spectral data to 8 partitions (1 partition for each rotating cube)
		int numPartitions = 1;
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
        float avg = newAverage(aveMag[0]);
        Debug.Log(avg);

        rend.material.SetFloat("_SwayAmount", avg);

        //if(avg > 0.7)
        //{
        //    dir *= -1;
        //    rend.material.SetFloat("_SwayDir", dir);
        //}

        ++index;
        if (index >= avgs.Length) index = 0;

		// --------- End animating cube via spectral data
		// --------------------------------------------------------



	}

    float newAverage(float avg)
    {
        sum -= avgs[index];
        avgs[index] = avg;
        sum += avgs[index];
        return (sum / avgs.Length);
    }


}

