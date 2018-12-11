using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class StorageUI : MonoBehaviour
{

    public GameObject[] playerDBSlots;
    private bool storageState = false;
    public int currentSlot;
    public Color[] colorSlots;
    public Color[] colorIcons;
    private GameObject storage;
    private GameObject player;
    private ObjectInteractor playerInteractor;
    public Image carryImage, carryIcon, lanterImage, lanternIcon;
    public bool lanternState = false;
    private bool lanternOn = false;
    public Sprite lantern;
    private GameObject maincamera;

    private float dPadXPrev = 0, dPadYPrev = 0;
    private bool dPadXReady = true, dPadYReady = true;

    [SerializeField] public Images images;
    [System.Serializable]
    public class Images
    {
        public Image[] slot, icon;
    }

    [SerializeField] public Icons icons;
    [System.Serializable]
    public class Icons
    {
        public Sprite[] icon;
        public string[] tag;
    }

    private void Awake()
    {
        maincamera = GameObject.FindWithTag("MainCamera");
        storage = GameObject.FindWithTag("Storage");
        storage.SetActive(storageState);
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerInteractor = player.GetComponent<ObjectInteractor>();
        }
        UpdateCurrentSlot();
    }

    void Update()
    {
        SetVisibilityState();
        if (storageState)
        {
            MoveThroughStorage();
            UpdateStorageIcons();
            UpdateCurrentSlot();
            RemoveItem();
            ChangeCarryObject();
            TurnLantern();
        }
    }

    void SetVisibilityState()
    {
        if (Input.GetButtonDown("Pad_Press"))
        {
            storageState = !storageState;
            storage.SetActive(storageState);
            playerInteractor.SetInteraction(!storageState);
        }
    }

    void UpdateStorageIcons()
    {
        for (int i = 0; i < images.icon.Length; i++)
        {
            if (playerDBSlots[i] != null)
            {
                for (int j = 0; j < icons.tag.Length; j++)
                {
                    if (playerDBSlots[i].tag == icons.tag[j])
                    {
                        images.icon[i].sprite = icons.icon[j];
                    }
                }
            }
            else
            {
                images.icon[i].sprite = null;
            }
        }

        if (playerInteractor.carryObject != null)
        {
            for (int i = 0; i < icons.tag.Length; i++)
            {
                if (playerInteractor.carryObject.tag == icons.tag[i])
                {
                    carryIcon.sprite = icons.icon[i];
                }
            }
        }
        else
        {
            carryIcon.sprite = null;
        }

        if (lanternState)
        {
            lanternIcon.sprite = lantern;
        }

    }

    void UpdateCurrentSlot()
    {
        for (int j = 0; j < images.slot.Length; j++)
        {
            if (j == currentSlot)
            {
                images.slot[currentSlot].color = colorSlots[0];
                images.icon[currentSlot].color = colorIcons[0];
            }
            else
            {
                images.slot[j].color = colorSlots[1];
                images.icon[j].color = colorIcons[1];

            }
        }
    }

    void MoveThroughStorage()
    {
        float dPadX = Input.GetAxisRaw("DPad_X");
        float dPadY = Input.GetAxisRaw("DPad_Y");
        const int numCols = 2;

        if (dPadXPrev != 0 && dPadX == 0)
            dPadXReady = true;
        if (dPadYPrev != 0 && dPadY == 0)
            dPadYReady = true;

        dPadXPrev = dPadX;
        dPadYPrev = dPadY;

        // Move DPad right (or mouse scroll)
        if ((dPadXReady && dPadX > 0) || Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            dPadXReady = false;
            currentSlot++;
            currentSlot %= playerDBSlots.Length;
        }

        // Move DPad left (or mouse scroll)
        if ((dPadXReady && dPadX < 0) || Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            dPadXReady = false;
            if (currentSlot > 0)
                currentSlot--;
            else
                currentSlot = playerDBSlots.Length - 1;
        }

        // Move DPad up
        if (dPadYReady && dPadY > 0)
        {
            dPadYReady = false;
            if (currentSlot >= numCols)
                currentSlot -= numCols;
            else
                currentSlot = playerDBSlots.Length + (currentSlot - numCols);
        }

        // Move DPad down
        if (dPadYReady && dPadY < 0)
        {
            dPadYReady = false;
            currentSlot += numCols;
            currentSlot %= playerDBSlots.Length;
        }
    }

    void RemoveItem()
    {
        if (Input.GetButtonDown("Button_Circle"))
        {
            if (playerDBSlots[currentSlot] != null)
            {
                playerDBSlots[currentSlot].SetActive(true);
                playerDBSlots[currentSlot].GetComponent<IInteractive>().ExecuteCarryAction("Drop", player);
                playerDBSlots[currentSlot] = null;
            }
        }
    }

    void ChangeCarryObject()
    {
        if (Input.GetButtonDown("Button_X"))
        {
            GameObject temp = playerInteractor.carryObject;

            playerInteractor.carryObject = playerDBSlots[currentSlot];
            if (playerInteractor.carryObject != null)
            {
                playerInteractor.carryObject.SetActive(true);
            }

            playerDBSlots[currentSlot] = temp;
            if (playerDBSlots[currentSlot] != null)
            {
                playerDBSlots[currentSlot].SetActive(false);
            }
        }
    }

    public bool GetEmptySlots()
    {
        for (int i = 0; i < playerDBSlots.Length; i++)
        {
            if (playerDBSlots[i] == null)
                return true;
        }
        return false;
    }

    public void UpdateStorage(GameObject obj)
    {
        for (int i = currentSlot; i <= playerDBSlots.Length; i++)
        {
            if (GetEmptySlots())
            {
                if (i == playerDBSlots.Length)
                {
                    i = -1;
                }
                else if (playerDBSlots[i] == null)
                {
                    playerDBSlots[i] = obj;
                    return;
                }
            }
        }
    }

    public bool GetStorageState()
    {
        return storageState;
    }

    public void SetActiveLantern()
    {
        lanternState = true;
        lanternOn = true;
    }

    public void TurnLantern()
    {
        if (lanternState)
        {
            if (Input.GetButtonDown("Button_Square"))
            {
                lanternOn = !lanternOn;
                if (lanternOn) {
                    maincamera.GetComponent<Light>().intensity = 7;
                    lanternIcon.color = colorIcons[2];
                    lanterImage.color = colorIcons[2];
                } else {
                    maincamera.GetComponent<Light>().intensity = 0;
                    lanternIcon.color = colorIcons[0];
                    lanterImage.color = colorIcons[0];
                }
            }
        }
    }

}