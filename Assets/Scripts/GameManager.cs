using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

	public bool MenuOpen { get; set; }
	public bool GameOver { get; set; }

	public List<WaveRider> WaveRiders;
	public ChargeManager chargeManager;
	public ScoreManager scoreManager;
	public MainWave wave;

	public void ProcessInputs() {
		if (MenuOpen) {
			//menuManager.ProcessInputs(p);
		}
		else {
			if(GameManager.Instance.GameOver && Input.GetButtonDown("Select")) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
			HandleUnpausedInputs();
		}
	}

	public void HandleUnpausedInputs() {
		var selectedWaveRiders = WaveRiders.Where(w => w.Selected);
		if (selectedWaveRiders.Count() > 0 && wave.AllowRiderMovement) {
			if(Input.GetMouseButton(0)) {
				Vector3 mp = Input.mousePosition;
				Vector2 mousePosition = Camera.main.ScreenToWorldPoint( new Vector3( mp.x, mp.y, wave.transform.position.z-Camera.main.transform.position.z) );

				float diff = mousePosition.x - selectedWaveRiders.First().transform.position.x;
				diff /= wave.MoveAmountPerUpdate.x; //diff is now how many indices to move

				int idiff = (int)diff;

				//diff is positive is mouse was moved to the right
				//the 0th index of the wavepoints is the rightmost point
				
				int maxima;
				if(idiff == 0) {
					return;
				}
				else if(idiff < 0) {
					maxima = WaveRiders.Max( w => w.RestingPointIndex);
					idiff = Mathf.Min(-idiff, (wave.WavePoints-1)-maxima);
				}
				else{
					maxima = WaveRiders.Min(w => w.RestingPointIndex);
					idiff = Mathf.Max(-idiff, 0-maxima);
				}

				idiff = (int) (Mathf.Sign(idiff) * Mathf.Min( chargeManager.Charge, Mathf.Abs(idiff) ));
				chargeManager.ChangeCharge(-Mathf.Abs(idiff));

				foreach(WaveRider w in WaveRiders) {
					w.Move(idiff);
				}
				
			}
			else {
				foreach (WaveRider w in selectedWaveRiders)
					w.Selected = false;
			}
		}
		else {
			
		}
	}

	public void Update() {
		ProcessInputs();
	}

	/*
	public GameObject playerPrefab;

	[HideInInspector]
	public Player player;

	public MenuManager menuManager;
	public Settings settings;

	///is a player active in the game
	public bool GameActive { get; set; }

	public bool MenuOpen { get; set; }

	public AudioSource InGameMusic;
	public AudioSource MenuMusic;

	public Background background;

	Coroutine crossfadeCoroutine;
	Coroutine loadStartMenuCoroutine;

	public void Awake() {
		settings = Settings.Load();		
	}

	void Start() {
		//get the audio sources for the in game music and menu music
		AudioSource[] sources = GetComponents<AudioSource>();
		InGameMusic = sources.First(s => s.clip.name != "Menu");
		MenuMusic = sources.First(s => s.clip.name == "Menu");

		background = GameObject.FindGameObjectWithTag("Background").GetComponent<Background>();

		if (!settings.Music) {
			InGameMusic.mute = true;
			MenuMusic.mute = true;
		}

		//load up the main menu
		ToMainMenu();

		//make sure the in game music is not playing because only the menu music should be playing
		InGameMusic.volume = 0f;
		MenuMusic.volume = 0f;

		//hide the cursor
		Cursor.visible = false;
	}

	//leaving the menu to enter gameplay
	public void ExitMenu() {
		MenuOpen = false;

		//restart music
		if ((MenuTypes)menuManager.Index == MenuTypes.Start) {
			if(crossfadeCoroutine != null) {
				StopCoroutine(crossfadeCoroutine);
			}
			//stop start menu load
			if (loadStartMenuCoroutine != null) {
				StopCoroutine(loadStartMenuCoroutine);
			}
			float start = MenuMusic.volume > 0.4f ? -1f : 0f;
			crossfadeCoroutine = StartCoroutine(Crossfade(InGameMusic, MenuMusic, start, MenuMusic.volume, 0.6f, 0f));
		}
		else if((MenuTypes)menuManager.Index == MenuTypes.Score) {
			InGameMusic.volume = 0.6f;
		}

		//leave whatever menu we are currently in
		menuManager.ExitMenu();

		//resume gameplay volume for all the blades active in the scene
		GameObject[] blades = GameObject.FindGameObjectsWithTag("Blade");
		foreach (GameObject o in blades) {
			AudioSource s = o.GetComponent<AudioSource>();
			Blade b = o.GetComponent<Blade>();

			s.volume = b.DefaultVolume;
		}

		//resume normal timeflow
		//when game is paused, time flow is slowed down
		Time.timeScale = 1f;
	}

	public void Pause() {
		MenuOpen = true;
		menuManager.EnterMenu(MenuTypes.Pause);
		Time.timeScale = 0.000001f;
	}

	public void StartGame() {
		//spawn player
		GameActive = true;
		player = Instantiate(playerPrefab).GetComponent<Player>();

		ExitMenu();
	}

	public void ExitToDesktop() {
		Application.Quit();
	}

	///going from playing the game to a menu where a player is not active
	public void ExitingGameplay() {
		MenuOpen = true;
		GameActive = false;

		//resume normal timescale 
		Time.timeScale = 1f;

		//remove player from scene
		if (player != null) {
			Destroy(player.gameObject);
		}
	}

	/// <summary>
	/// Go to the start menu
	/// </summary>
	/// <param name="fromGameplay">Are we coming to the menu from playing the game or is this the first load of the start menu</param>
	public void ToMainMenu(bool fromGameplay = false) {
		ExitingGameplay();
		menuManager.EnterMenu(MenuTypes.Start);

		if (crossfadeCoroutine != null) {
			StopCoroutine(crossfadeCoroutine);
		}
		loadStartMenuCoroutine = StartCoroutine("LoadStartMenu");
	}

	/// <summary>
	/// Player died
	/// </summary>
	public void GameOver() {		
		ExitingGameplay();

		if (crossfadeCoroutine != null) {
			StopCoroutine(crossfadeCoroutine);
		}
		MenuMusic.Stop();
		InGameMusic.volume = 0.3f;

		menuManager.EnterMenu(MenuTypes.Score);
	}

	public void ProcessInputs(InputPackage p){
		if(MenuOpen) {
			menuManager.ProcessInputs(p);
		}
		else {
			if(p.Pause) {
				Pause();
			}
			else {
				player.SetMoveDirections(p.Horizontal, p.Vertical);
			}	
		}
	}

	IEnumerator LoadStartMenu(){
		MenuMusic.volume = 0;
		InGameMusic.volume = 0;

		yield return StartCoroutine((menuManager.ActiveMenu as MainMenu).ShowTitle());

		if(MenuOpen)
			crossfadeCoroutine = StartCoroutine(Crossfade(MenuMusic, InGameMusic, 0f, 0f, 0.5f, 0f));
	}

	//crossfade 2 audio sources
	//used to change between menu music and in-game music
	public IEnumerator Crossfade(AudioSource coming, AudioSource going, float comingStartVolume = -10f, float goingStartVolume = -10f, float comingEndVolume = -10f, float goingEndVolume = -10f) {
		float startTime = Time.time;

		float comingvolume = coming != null ? coming.volume : 0f;
		float goingvolume = going != null ? going.volume : 0f;

		comingStartVolume = comingStartVolume < -9f ? comingvolume : comingStartVolume;
		goingStartVolume = goingStartVolume < -9f ? goingvolume : goingStartVolume;

		comingEndVolume = comingEndVolume < -9f ? goingvolume : comingEndVolume;
		goingEndVolume = goingEndVolume < -9f ? comingvolume : goingEndVolume;

		coming.Play();

		float fadeTime = 2f;

		while(Time.time - startTime <= fadeTime) {
			float ttime = (Time.time - startTime) / fadeTime;

			if(coming != null)
				coming.volume = Mathf.Lerp(comingStartVolume, comingEndVolume, ttime);

			if(going != null)
				going.volume = Mathf.Lerp(goingStartVolume, goingEndVolume, ttime);

			yield return new WaitForEndOfFrame();
		}

		going.Stop();
	}
	*/
}