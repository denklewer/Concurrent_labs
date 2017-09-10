// ConcLab25.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <stdio.h>
#include <mpi.h>
#include <iostream>
#include <cmath>
#include <string>
#include <fstream>
#include <ctime>
#include <sstream>
#include <vector>
#include <iterator>
#include <algorithm>
#include <clocale>
 const int ArgNum = 10;
struct Individuo {
public:
	double  genes[ArgNum];
	double fitness;


	int size;


};

// Возвращает псевдослучайное число из диапазона [min, max)
int Random(int min, int max) {
	return  min + (rand() % (int)(max - min + 1));
	
}
double Random(double min, double max) {

	return (max - min) * ((double)rand() / (double)RAND_MAX) + min;
}
 double F(Individuo ind) {
	 double sum=0;
	 for (size_t i = 0; i < ind.size; i++)
	 {
		 sum = sum + pow((ind.genes[i] - 1), 2);
	 }
	 return sum;
}
 double Fitness(Individuo ind) {
	 return 1 / (1 + F(ind));
 }

 std::vector<Individuo> Initialize(int argCount) {
	 std::vector<Individuo> P;
	 for (size_t i = 0; i < 1000; i++)
	 {
		 Individuo tmp;
		 tmp.size = argCount;
		// tmp.genes = double[tmp.size];//+
		 for (size_t j = 0; j < argCount; j++)
		 {
			 tmp.genes[j]=(Random(-100, 100));
		 }
		 tmp.fitness = Fitness(tmp);
		 P.push_back(tmp);
	 }
	 return P;
 }



std::vector<Individuo> Selection(std::vector<Individuo> P,int ExCount) {
	for (size_t i = 0; i < ExCount; i++)
	{

	
	int ind1 = Random(0, P.size()-1);
	int ind2 = Random(0, P.size()-1);


	if (P[ind1].fitness > P[ind2].fitness) {


		Individuo* tmp= new Individuo(P[ind1]);

		//tmp.genes = new double[P[ind1].size];//+
		/*tmp.fitness = P[ind1].fitness;
		tmp.size = P[ind1].size;
		for (size_t i = 0; i < tmp.size; i++)
		{
			tmp.genes[i] = P[ind1].genes[i];
		}*/
			
		P[ind2] = *tmp;
	}
	else {
		Individuo* tmp= new Individuo(P[ind2]);
		//tmp.genes = new double[P[ind2].size];//+
		
		P[ind1] = *tmp;

	}
	}
	return P;
}
std::vector<Individuo>  Crossover(std::vector<Individuo> P, int ExCount)
{
	for (size_t k = 0; k < ExCount; k++)
	{
		int ind1 = Random(0, P.size() - 1);
		int ind2 = Random(0, P.size() - 1);
		int pos = Random(0, P[ind1].size);
		double tmp = 0;
		for (size_t i = pos; i < P[ind1].size; i++)
		{

			tmp = P[ind1].genes[i];
			P[ind1].genes[i] = P[ind2].genes[i];
			P[ind2].genes[i] = tmp;
		}
		P[ind1].fitness = Fitness(P[ind1]);
		P[ind2].fitness = Fitness(P[ind2]);
	}
	return P;
}
std::vector<Individuo> Mutate(std::vector<Individuo> P) {
	for (size_t i = 0; i < P.size(); i++)
	{
		for (size_t j = 0; j < P[i].size; j++)
		{
			if (Random(0.0,1.0)<0.1) {
				P[i].genes[j] += Random(-0.05, 0.05);
			}
		}
		P[i].fitness = Fitness(P[i]);

	}

	return P;
}

double* Averange(std::vector<Individuo> P) {
	double* av= new double[P[0].size];
	double sum=0;
	for (size_t j = 0; j < P[0].size; j++)
	{
		for (size_t i = 0; i < P.size(); i++)
		{
			sum += P[i].genes[j];

		}
		av[j] =(sum / P.size());
		sum = 0;
	}
	return av;
}

int main(int argc, char** argv)
{
	
	setlocale(LC_CTYPE, "rus");
	int mode = 1;
	//количество аргументов
	//const int ArgNum = 100;
	auto time1 = clock();
	if (mode == 1) {
	
	std::vector<Individuo> P = Initialize(ArgNum);
	int N = 1;
	int N_MAX = 100000;
	double* q;
	while (N < N_MAX)
	{

		P = Selection(P,ArgNum);
		P = Crossover(P,ArgNum);
		P = Mutate(P);
		q = Averange(P);
		Individuo test;	
		for (size_t x = 0; x < ArgNum; x++)
		{
			test.genes[x] = q[x];
		}
		test.size = ArgNum;
		printf(" Поколение %d --", N);
		printf("Значение функции %f -- фитнесс %f \n", F(test), Fitness(test));
		N += 1;
		if (F(test) < 0.1) {
			printf("закончил за %d поколений", N);
			break;
		}
	}
	auto time2 = clock();
	double r = time2 - time1;
	printf("time: %f \n\r", (r / CLOCKS_PER_SEC));
	system("pause");

	}
	//многопоточная версия
	else {
		

		// *********************************
		// для создания типа
		//**********************************
			printf("Зашел");
		MPI_Datatype Individuo_type;
		MPI_Datatype typelist[3] = { MPI_DOUBLE,MPI_DOUBLE,MPI_INT };
		int block_lengths[3];
		MPI_Aint displacements[3];
		int blocks_number;
		int tag = 0;
		//**********************************
		MPI_Init(&argc, &argv);
		int myrank;
		MPI_Status status;
		MPI_Comm_rank(MPI_COMM_WORLD, &myrank);
		std::vector<Individuo> P = Initialize(ArgNum);

		//**************************************************
		//Создание типа
		//**************************************************		
		block_lengths[0] = ArgNum;
		block_lengths[1] = 1;
		block_lengths[2] = 1;
		int  j = 1;
		displacements[0] = offsetof(Individuo, genes);
		displacements[1] = offsetof(Individuo, fitness);
		displacements[2] = offsetof(Individuo, size);
		blocks_number = 3;
		MPI_Type_create_struct(blocks_number, block_lengths, displacements, typelist, &Individuo_type);
		MPI_Type_commit(&Individuo_type);
	
		//*********************************************

		int N = 1;
		int N_MAX = 100000;
		double* q;
		while (N < N_MAX)
		{			
			if (N %1000!=0) {		
				P = Selection(P,ArgNum);			
				P = Crossover(P,ArgNum);				
				P = Mutate(P);				
				q = Averange(P);				
				Individuo test;
				for (size_t x = 0; x < ArgNum; x++)
				{
					test.genes[x] = q[x];
				}
				test.size = ArgNum;
				if (F(test) < 0.1) {
					printf("%d закончил за %d поколений\n", myrank,N);
					break;
				}
			
				printf("%d Поколение %d --\n",myrank, N);
			//	printf("Значение функции %f -- фитнесс %f \n", F(test), Fitness(test));
				N += 1;
			}
			else {
				N++;
				if (myrank == 0)
				{
				   //главный поток
					std::vector<Individuo> tmpVector;
					int size = P.size();
					printf("%d Запишу элементов %d --\n", myrank, size/10);		
					int* indexes = new int [size/10];
					for (size_t i = 0; i < size / 10; i++)
					{
						int maxInd = 0;
						double maxVal = P[0].fitness;
						int j = 0;
						//ищем значения с максимальной fitness
						while (j < P.size()) {
							double tmp = P[j].fitness;
							if (tmp > maxVal) {
								maxInd = j;
								maxVal = tmp;
							}
							j++;
						}
						j = 0;
						
						/*********************/

					/*	Individuo tmp2 = Individuo();
						
						tmp2.fitness = P[maxInd].fitness;
						tmp2.size = P[maxInd].size;
						for (size_t k = 0; k < tmp2.size; k++)
						{
							tmp2.genes[k] = P[maxInd].genes[k];
						}

						*/
						indexes[i] = maxInd;
						tmpVector.push_back(*new Individuo(P[maxInd]));
						P[maxInd].fitness = 0;
						// удаление элемента из вектора
						/*P.erase(P.begin() + maxInd);
						std::vector<Individuo>(P).swap(P);*/
					}
					
				
				
				
					MPI_Comm_size(MPI_COMM_WORLD, &size);
					size = tmpVector.size()*(size - 1);
			
			
					printf("%d Приму сообщений %d --\n", myrank, size);
				
					for (size_t w = 0; w < size; w++)
					{
						Individuo ResTmp;											
						MPI_Recv(&ResTmp, 1, Individuo_type, MPI_ANY_SOURCE, MPI_ANY_TAG, MPI_COMM_WORLD, &status);
						// копирование полученного в новую переменную	
						/*Individuo tmp2 = Individuo();					
						tmp2.fitness = ResTmp.fitness;
						tmp2.size = ResTmp.size;					
						for (size_t k = 0; k < tmp2.size; k++)
						{
							tmp2.genes[k] =ResTmp.genes[k];
						}*/
						//**********************************************
						tmpVector.push_back(*new Individuo(ResTmp));
					}
					printf("Все сообщения получены  в векторе %d элементов\n", tmpVector.size());
			
				
					int prCount = 0;
					int r = 0;
					MPI_Comm_size(MPI_COMM_WORLD, &prCount);

					size = tmpVector.size() / prCount;
					for (size_t rank = 1; rank < prCount; rank++)
					{
			
						printf("%d Отправляю %d обратно %d сообщений --\n", myrank,rank, size);
				
					
						for (size_t b = 0; b < size; b++)
						{
					
							r = Random(0, tmpVector.size()-1);	
						
							
							/*Individuo tmp2 = Individuo();
							tmp2.fitness = tmpVector[r].fitness;
							tmp2.size = tmpVector[r].size;
							for (size_t k = 0; k < tmp2.size; k++)
							{
								tmp2.genes[k] = tmpVector[r].genes[k];
							}*/
							MPI_Send((new Individuo(tmpVector[r])), 1, Individuo_type, rank, tag, MPI_COMM_WORLD);
							
							tmpVector.erase(tmpVector.begin() + r);
							std::vector<Individuo>(tmpVector).swap(tmpVector);
					
						}
	
				
				
					}

					for (size_t j = 0; j < size; j++)
					{	// копирование полученного в новую переменную
					/*	Individuo tmp2 = Individuo();
						tmp2.fitness = tmpVector[j].fitness;
						tmp2.size = tmpVector[j].size;
						for (size_t k = 0; k < tmp2.size; k++)
						{
							tmp2.genes[k] = tmpVector[j].genes[k];*/
						//*********************************************
						P[indexes[j]] = *new Individuo(tmpVector[j]);
					//	P.push_back(*new Individuo(tmpVector[j]));
					}
					printf("%d Все сообщения отправлены  в векторе %d элементов\n", myrank, P.size());
				}
				else {
					// побочный поток
					std::vector<Individuo> tmpVector;
					int size = P.size();
					printf("%d Запишу элементов %d --\n", myrank, size/10);
				
					int* indexes = new int[size / 10];
					for (size_t i = 0; i < size / 10; i++)
					{
						int maxInd = 0;
						double maxVal = P[0].fitness;
						int j = 0;
						while (j < P.size()) {
							double tmp = P[j].fitness;
							if (tmp > maxVal) {
								maxInd = j;
								maxVal = tmp;
							}
							j++;
						}

					/*	Individuo tmp2 = Individuo();
						tmp2.fitness = P[maxInd].fitness;
						tmp2.size = P[maxInd].size;
						for (size_t k = 0; k < tmp2.size; k++)
						{
							tmp2.genes[k] = P[maxInd].genes[k];
						}*/
						indexes[i] = maxInd;
						tmpVector.push_back(*new Individuo(P[maxInd]));
						P[maxInd].fitness = 0;
						// удаление элемента из вектора
						/*P.erase(P.begin() + maxInd);
						
						std::vector<Individuo>(P).swap(P);*/
					}
					printf("%d Отправлю сообщений %d --\n", myrank, tmpVector.size());
				
					for (size_t i = 0; i < tmpVector.size(); i++)
					{
						
						/*Individuo tmp2 = Individuo();
						tmp2.fitness = tmpVector[i].fitness;
						tmp2.size = tmpVector[i].size;
						for (size_t k = 0; k < tmp2.size; k++)
						{
							tmp2.genes[k] = tmpVector[i].genes[k];
						}
						*/
						MPI_Send(new Individuo(tmpVector[i]), 1, Individuo_type, 0, tag, MPI_COMM_WORLD);
					}
					printf("Все сообщения от потока %d отправлены \n", myrank);
				

					printf("%d Приму сообщений обратно %d --\n", myrank, size/10);
				
					for (size_t i = 0; i < size/10; i++)
					{
						Individuo tmp2;
						MPI_Recv(&tmp2, 1, Individuo_type, 0, tag, MPI_COMM_WORLD, &status);
				
						P[indexes[i]] = *new Individuo(tmp2);
						//P.push_back(*new Individuo(tmp2));
					}
					printf("%d Все сообщения получены  в векторе %d элементов\n",myrank, P.size());
			
				
				}
			}
		}
		auto time2 = clock();
		double r = time2 - time1;
		printf("time: %f \n\r", (r / CLOCKS_PER_SEC));
		MPI_Type_free(&Individuo_type);
		MPI_Finalize();
	
	}
	/*MPI_Init(&argc,&argv);
	int *buffer;
	int myrank;
	MPI_Status status;
	int N = 1;
	MPI_Comm_rank(MPI_COMM_WORLD, &myrank);
	if (myrank == 0)
	{
		buffer = (int *)malloc(N + MPI_BSEND_OVERHEAD);
		MPI_Buffer_attach(buffer, N + MPI_BSEND_OVERHEAD);
		buffer = (int *)10;
		MPI_Bsend(&buffer, N, MPI_INT, 1, 0, MPI_COMM_WORLD);
		MPI_Buffer_detach(&buffer, &N);
	}
	else
	{
		MPI_Recv(&buffer, N, MPI_INT, 0, 0,
			MPI_COMM_WORLD, &status);
		printf("received: %i\n", buffer); 

	}
	MPI_Finalize();*/
}