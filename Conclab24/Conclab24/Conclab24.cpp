// Conclab24.cpp: определяет точку входа для консольного приложения.
//
#pragma once
#include "stdafx.h"
#include<omp.h> 
#include <string>
#include <fstream>
#include <cmath>
#include <iostream>
#include <ctime>

#include <string>
#include <sstream>
#include <vector>
#include <iterator>
#include <algorithm>
#include <clocale>
template<typename Out>

void split(const std::string &s, char delim, Out result) {
	std::stringstream ss;
	ss.str(s);
	std::string item;
	while (std::getline(ss, item, delim)) {
		*(result++) = item;
	}
}

std::vector<std::string> split(const std::string &s, char delim) {
	std::vector<std::string> elems;
	split(s, delim, std::back_inserter(elems));
	return elems;
}

 class DataEntry {
 public :
	std::vector<double> attributes;
	 int EntryClass;
	
	 DataEntry(std::vector<std::string> input) {
		//attributes = new double[input.size() - 1];
		for (int i = 0; i < input.size() - 1; i++) {
			attributes.push_back( atof(input[i].c_str()));
			//attributes[i] =(atof(input[i].c_str()));
		}
	
		EntryClass =atoi(input[input.size() - 1].c_str());
		

	}
	 DataEntry() {
		 EntryClass = 0;
		 
	 }
 };

 size_t TrainingDataSize;
 size_t TestDataSize;
 DataEntry* TrainingData;
 DataEntry* TestData;
 std::string datainfo;
 void ReadData(char* path, int rowNumber) {
	 std::vector<std::string> row;
	 int count4 = 0;
	 int count2 = 0;
	 std::string s = path;
	 // var rows = File.ReadLines(path, System.Text.Encoding.Default);
	 std::vector<std::string> rows;
	 std::ifstream file(s, std::ios::in);

	 while (!(file.eof()) && rowNumber>0)
	 {
		 getline(file, s);
	
		 rows.push_back(s);
		 rowNumber--;

	 };
	 file.close();
	 if (rowNumber > 0) {
		 std::cout << "Количество строк, указанное вами больше, чем размер файла. Считан весь файл", "Предупреждение \n\r";
	 }




	 TrainingDataSize = rows.size() / 2;
	 TrainingData = new DataEntry[rows.size() / 2];

	 for (int i = 0; i<rows.size() / 2; i++) {
		 row = split(rows[i], ',');

		 TrainingData[i] = *(new DataEntry(row));


		 if (TrainingData[i].EntryClass == 2)
		 {
			 count2++;

		 }

		 if (TrainingData[i].EntryClass == 4)
		 {
			 count4++;

		 }


	 }

	 datainfo = datainfo + "Training: 2-" + std::to_string(count2) + ";4-" + std::to_string(count4) + "\n\r";
	 count2 = 0;
	 count4 = 0;
	 TestDataSize = rows.size() - rows.size() / 2;
	 TestData = new DataEntry[rows.size() - rows.size() / 2];
	 int j = 0;
	 for (int i = rows.size() / 2; i < rows.size(); i++)
	 {
		 row = split(rows[i], ',');

		 TestData[j] = *(new DataEntry(row));

		 if (TestData[j].EntryClass == 2) {
			 count2++;

		 }

		 if (TestData[j].EntryClass == 4)
		 {
			 count4++;

		 }
		 j++;
	 }

	 datainfo = datainfo + "Test: 2-" + std::to_string(count2) + ";4-" + std::to_string(count4) + '\n';
	 std::cout << datainfo;
	 count2 = 0;
	 count4 = 0;



 }

  void NormalizeTraining() {
	  int sz = TrainingData[0].attributes.size();
	 double* mins = new double[sz];
	 double* maxs = new double[sz];

	 for (size_t i = 0; i <sz; i++) {
		 double min = TrainingData[0].attributes[i];
		 double max = TrainingData[0].attributes[i];

		 for (size_t j = 1; j < TrainingDataSize;j++) {
			 if (TrainingData[j].attributes[i]<min) {
				 min = TrainingData[j].attributes[i];			      
			 }
			 if (TrainingData[j].attributes[i]>max) {
				 max = TrainingData[j].attributes[i];				 
			 }		 
		 }

		 mins[i] =min;
		 maxs[i] =max;
	 }
	 for (int j = 0; j<TrainingDataSize; j++)
	 {
		 for (int i = 0; i < TrainingData[j].attributes.size(); i++)
		 {
			 TrainingData[j].attributes[i] = (TrainingData[j].attributes[i] - mins[i]) / (maxs[i] - mins[i]);
		 }
	 }

 }

  void NormalizeTest() {
	  int sz = TestData[0].attributes.size();
	  double* mins = new double[sz];
	  double* maxs = new double[sz];

	  for (size_t i = 0; i <sz; i++) {
		  double min = TestData[0].attributes[i];
		  double max = TestData[0].attributes[i];

		  for (size_t j = 1; j < TestDataSize; j++) {
			  if (TestData[j].attributes[i]<min) {
				  min = TestData[j].attributes[i];
			  }
			  if (TestData[j].attributes[i]>max) {
				  max = TestData[j].attributes[i];
			  }
		  }

		  mins[i] = min;
		  maxs[i] = max;
	  }
	  for (int j = 0; j<TestDataSize; j++)
	  {
		  for (int i = 0; i < TestData[j].attributes.size(); i++)
		  {
			  TestData[j].attributes[i] = (TestData[j].attributes[i] - mins[i]) / (maxs[i] - mins[i]);
		  }
	  }

  }


   double Distance(DataEntry a, DataEntry b) {
	  double res = 0;
	  /*int id = omp_get_thread_num();
	  int numt = omp_get_num_threads();
	  printf("Thread(%d) of(%d) threads alive\n", id, numt);*/
	  for (int i = 0; i < a.attributes.size(); i++) {
		  res = res + std::pow((a.attributes[i] - b.attributes[i]), 2);
	  }
	  
	  res = std::sqrt(res);
	  return res;

  }

   double vote_fun(DataEntry* neighbours, int nbCount, int DataClass, DataEntry x) {
	   double res = 0;
	   for (size_t i = 0; i < nbCount; i++) {
		   if (neighbours[i].EntryClass == DataClass) {
			   res = res + 1 / std::pow(Distance(x, neighbours[i]), 2);
		   }
		   return res;
	   }
   }


    int KNN(int k, DataEntry o) {
		DataEntry* neighbours = new DataEntry[k];

		std::vector<double> distances;

		double maxDist = 0;

		double Dist = 0;

		
		 
		for (size_t j = 0; j < TrainingDataSize; j++) {
			Dist = Distance(TrainingData[j], o);
			distances.push_back(Dist);
			if (Dist > maxDist) maxDist = Dist;
		}
		for (size_t i = 0; i < k; i++) {
			double min = distances[0];
			size_t min_ind = 0;
			for (size_t j = 1; j < distances.size();j++) {
				if (distances[j] < min) {
					min = distances[j];
					min_ind = j;
				}						
			}
					neighbours[i] = TrainingData[min_ind];
					
					distances[min_ind] = maxDist;
			}
							   
		   
	   int MaxClass = 0;
	   double MaxVoteRes = 0;
	   double tmp;
	   std::vector<int> Classes = { 2, 4 }; // Явно задано!!
	
	   for (int i = 0; i < Classes.size();i++) {
		   tmp = vote_fun(neighbours,k, Classes[i], o);
		   if (tmp > MaxVoteRes) {
			   MaxClass = Classes[i];
			   MaxVoteRes = tmp;

		   }
	   }

	   return MaxClass;
   }

	int KNNParallel(int k, DataEntry o) {
		DataEntry* neighbours = new DataEntry[k];

		std::vector<double> distances;

		double maxDist = 0;
		double Dist = 0;
		int j = 0;
		int cnt = 0;
		#pragma omp parallel for shared (maxDist) private (j,Dist)
		for ( j = 0; j < TrainingDataSize; j++) {
			Dist = Distance(TrainingData[j], o);
            #pragma omp critical
			distances.push_back(Dist);
		
			#pragma omp critical
			if (Dist > maxDist) maxDist = Dist;
		}
	
		for (size_t i = 0; i < k; i++) {
			double min = distances[0];
			size_t min_ind = 0;
			for (size_t j = 1; j < distances.size(); j++) {
				if (distances[j] < min) {
					min = distances[j];
					min_ind = j;
				}
			}
			neighbours[i] = TrainingData[min_ind];

			distances[min_ind] = maxDist;
		}


		int MaxClass = 0;
		double MaxVoteRes = 0;
		double tmp;
		std::vector<int> Classes = { 2, 4 }; // Явно задано!!

		for (int i = 0; i < Classes.size(); i++) {
			tmp = vote_fun(neighbours, k, Classes[i], o);
			if (tmp > MaxVoteRes) {
				MaxClass = Classes[i];
				MaxVoteRes = tmp;

			}
		}

		return MaxClass;
	}
	class A {
	public:
		void method1();
	protected :
		void method2();
	private:
		void method3();
	
	};
	class B:public A {
	public:
		void method1() {};
		void method2() {};
		void method3() {};
	};
	union Types {
		char a;
		short int b;
		int c;
		double d;
		


	};

int main()
	{
	setlocale(LC_CTYPE, "rus");
	Types h;
	printf("%d", sizeof(Types));

	/*int id = omp_get_thread_num();
	int numt = omp_get_num_threads();
	printf("Thread(%d) of(%d) threads alive\n", id, numt);
	
	int rowNum = 1000;
	ReadData("D:\\C#\\data mining\\input.data",rowNum);
	NormalizeTraining();
	NormalizeTest();

	 int TP = 0;
	 int TN = 0;
	 int FP = 0;
	 int FN = 0;


	 int TPP = 0;
	 int TNP = 0;
	 int FPP = 0;
	 int FNP = 0;
	 auto time1 =clock();
     //#pragma parallel for 
	 for (int i = 0; i < TestDataSize; i++)
	 {

		 int a = TestData[i].EntryClass;
		// int b = KNN(4, TestData[i]);
		 int b = KNNParallel(4, TestData[i]);


		 if (a == 4 & b == 4) TP++;
		 if (a == 2 & b == 2) TN++;
		 if (a == 2 & b == 4) FP++;
		 if (a == 4 & b == 2) FN++;

		 ///printf("%d) Настоящий класс: %d | Предсказанный %d \n\r", i, a, b);


	 }
	 auto time2 = clock();

	 printf("TP - %d | FP - %d \n\r FN - %d | TN \ %d \n\r", TP, FP, FN, TN);
	 double r = time2 - time1;
	 printf("time: %f \n\r",(r/CLOCKS_PER_SEC));
	 */
	system("pause");
	
    return 0;
}

