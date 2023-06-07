#!/bin/bash
pkill -9 -f Peasie
pkill -9 -f Magazaki
pkill -9 -f Nestrix
rm PeasieAPI.out
rm ./Apps/PeasieAPI/logs/*
rm MagazakiAPI.out
rm ./Magazaki/Apps/WebShopAPI/logs/*
rm NestrixAPI.out
rm ./Nestrix/Apps/BankingRestAPI/logs/*