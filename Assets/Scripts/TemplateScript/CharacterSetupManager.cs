using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSetupManager : MonoBehaviour {

    public const int mTotal = 12;
    public int mPointLeft
    {
        get { return mTotal - (mStrength + mVitality + mAgility); }
    }
    public int mStrength = 0;
    public int mVitality = 0;
    public int mAgility = 0;

    public Slider strSlider;
    public Slider vitSlider;
    public Slider agiSlider;
    public Text   strText;
    public Text   vitText;
    public Text   agiText;

    public Text pointsLeft;
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        strText = strSlider.transform.FindChild("Value").GetComponent<Text>();
        vitText = vitSlider.transform.FindChild("Value").GetComponent<Text>();
        agiText = agiSlider.transform.FindChild("Value").GetComponent<Text>();
        setPointsLeftText();
    }

    public void setStr(float value)
    {
        if ((int)value + mVitality + mAgility > mTotal)
        {
            mStrength = mTotal - (mVitality + mAgility);
            strSlider.value = (float)mStrength;
        }
        else mStrength = (int)value;
        setPointsLeftText();
    }

    public void setVit(float value)
    {
        if ((int)value + mStrength + mAgility > mTotal)
        {
            mVitality = mTotal - (mStrength + mAgility);
            vitSlider.value = (float)mVitality;
        }
        else mVitality = (int)value;
        setPointsLeftText();
    }

    public void setAgi(float value)
    {
        if ((int)value + mVitality + mStrength > mTotal)
        {
            mAgility = mTotal - (mVitality + mStrength);
            agiSlider.value = (float)mAgility;
        }
        else mAgility = (int)value;
        setPointsLeftText();
    }

    public void readyToGo()
    {
        strSlider = null;
        vitSlider = null;
        agiSlider = null;
        strText = null;
        vitText = null;
        agiText = null;
        pointsLeft = null;
        Application.LoadLevel("battlefieldtest");
    }

    private void setPointsLeftText()
    {
        pointsLeft.text = mPointLeft.ToString();
        strText.text = mStrength.ToString();
        vitText.text = mVitality.ToString();
        agiText.text = mAgility.ToString();
    }
}
