-- Criar o banco de dados
CREATE DATABASE Biblioteca;

-- Criar tabela Aluno
CREATE TABLE aluno (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(100) not null,
    cpf VARCHAR(20) not null,
    email VARCHAR(100) not null,
    telefone VARCHAR(20),
    celular VARCHAR(20) not null,
    data_nascimento DATE not null
);

-- Criar tabela Livro
CREATE TABLE livro (
    id INT IDENTITY(1,1) PRIMARY KEY,
    titulo VARCHAR(100)  not null,
    autor VARCHAR(100)  not null,
    numero_pagina INT  not null,
    preco DECIMAL(10, 2)  not null,
    ano_publicacao INT  not null,
    isbn VARCHAR(20)  not null
);
