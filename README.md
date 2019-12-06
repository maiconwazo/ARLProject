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
- Registre a biblioteca gerada e algumas referências do ARCore como dependência no arquivo `build.grandle` do aplicativo

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

- Configure a versão minima do android para a 24 e versão de compilação para última disponível
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
