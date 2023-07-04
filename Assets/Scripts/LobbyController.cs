using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button buttonPlay;
    public Button Music;
    public Button Sound;
    public Image spriteImageM;
    public Image spriteImageS;

    public Sprite newMusicSprite;
    public Sprite defaultMusicSprite;
    public Sprite newSoundSprite;
    public Sprite defaultSoundSprite;

    private bool isDefaultMusicSprite = true;
    private bool isDefaultSoundSprite = true;


    private void Start()
    {
        buttonPlay.onClick.AddListener(GamePlay);
        Music.onClick.AddListener(ChangeMusicSprite);
        Sound.onClick.AddListener(ChangeSoundSprite);
    }
    
    private void ChangeSoundSprite()
    {
        if (isDefaultSoundSprite)
        {
            spriteImageS.sprite = newSoundSprite;
            SoundManager.Instance.StopSfx();
        }
        else
        {
            spriteImageS.sprite = defaultSoundSprite;
            SoundManager.Instance.StartSfx();
        }
        isDefaultSoundSprite = !isDefaultSoundSprite;
    }

    private void ChangeMusicSprite()
    {
        if (isDefaultMusicSprite)
        {
            spriteImageM.sprite = newMusicSprite;
            SoundManager.Instance.StopMusic();
        }
        else
        {
            spriteImageM.sprite = defaultMusicSprite;
            SoundManager.Instance.StartMusic();
        }
        isDefaultMusicSprite = !isDefaultMusicSprite;
    }  

    private void GamePlay()
    {
        SoundManager.Instance.PlaySound(Sounds.PlayButtonClick);
        SceneManager.LoadScene(1);
    }
}
