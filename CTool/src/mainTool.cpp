
#include <stdio.h>

#include <esat/window.h>
#include <esat/draw.h>
#include <esat/input.h>
#include <esat/sprite.h>
#include <esat/time.h>

#include <loader.h>
#include <drawableEntity.h>
#include <sound.h>

#include <cstdlib>
#include <time.h>
#include <iostream>

unsigned char fps = 64; //Control de frames por segundo
double current_time, last_time;


// --- Sound Manager ---
AudioManager audio;
int backgroundMusic = -1;
int nightMusic = -1;
int tabernMusic = -1;
bool outside = true;
std::vector<int> enemyMusicIdList = {};
// --- *** ---



int esat::main(int argc, char** argv) {
	srand(time(NULL));
	esat::WindowInit(1024, 768);

	float dt = 0.125f;

	while (esat::WindowIsOpened() && !esat::IsSpecialKeyDown(esat::kSpecialKey_Escape)) {

		last_time = esat::Time();
		esat::DrawBegin();
		esat::DrawClear(0, 0, 0);



		esat::DrawEnd();
		do {
			current_time = esat::Time();
		} while ((current_time - last_time) <= 1000.0 / fps);
		esat::WindowFrame();
	}

	audio.Close();

	return 0;
}