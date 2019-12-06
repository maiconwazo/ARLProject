# Biblioteca ARL
Desenvolvimento TCC 2019/2

## Instalação

### Gerar arquivos
- Clonar o repositório para uma pasta local;
- Compilar o projeto `AndroidLayer\UnityLayer\build.gradle` para gerar um arquivo `.aar`.

### Importar biblioteca
- Dentro da sua aplicação, vá em `File > New > New module`;
- Selecione a opção `Import .JAR/.AAR Package`;
- No campo de arquivo selecione o .aar gerado anteriormente (geralmente em `AndroidLayer\UnityLayer\build\outputs\aar\UnityLayer.aar`;
- Dê um nome ou deixe o nome padrão para o subprojeto;
- Registre a biblioteca gerada e algumas referências do ARCore como dependência no arquivo `build.grandle` do aplicativo:

```
dependencies {
    ...
    implementation 'com.google.ar:core:1.13.0'
    implementation(name: 'arcore_unity', ext:'aar')
    implementation(name: 'google_ar_required', ext:'aar')
    implementation(name: 'unityandroidpermissions', ext:'aar')
    implementation(name: 'unitygar', ext:'aar')
    implementation project(":UnityLayer")
}
```

- Cire uma pasta chamada `libs` dentro do diretório da aplicação e copie todos os arquivos da pasta `AndroidLayer/UnityLayer/libs/` para dentro dela;
- Configure a versão minima do android para a 24 e versão de compilação para última disponível;
- No arquivo `build.grandle` adicionar:

```
allprojects {
    repositories {
        ...
        flatDir {
            dirs 'libs'
        }
        
    }
}
```

- Rode o comando `Sync`

## Implementação
- Após tudo estar configurado, o primeiro passo é criar uma classe para implementar a interface `IItem` e outra para `IRepository` retornando dentro do método `getItems(float latitude, float longitude)` os objetos próximos as coordenadas passadas por parâmetro;
- Na classe `main_activity.java`, instancie o repositório desenvolvido no passo anterior;
- Adicione um botão na tela inicial (no xml correspondente à `main_activity.java`);
- Faça `binding` do botão dentro de uma variável;
- Crie uma instância da classe ARLWorld passando como parâmetro o contexto atual, o botão de inicialização e o repositório instanciado.

```
    IRepository repositorio = new ConcreteRepository(); // instanciando repositório
    Button btnCamera = (Button) findViewById(R.id.btnCamera); // binding do botão

    ARLWorld arlWorld = new ARLWorld(this, btnCamera, repository); // instanciando a aplicação
```

- Faça build da aplicação e deploy no dispositivo móvel;
- Clique no botão da tela inicial.

A aplicação iniciará a execução chamando a cada 5 segundos o metódo para escanear os objetos próximos.
