OS := $(shell uname -s)

ifeq ($(OS),Linux)
	LIB_EXT = .so
    TEST_COVERAGE_FLAGS = --coverage
    RUNTIME_ID=linux-x64
	PATH_TAIL=$(BIN_PATH)/$(RUNTIME_ID)/publish
    EXECUTABLE=$(APP_DIR)/$(PATH_TAIL)/ConsoleInterface
else
	LIB_EXT = .dll
    TEST_COVERAGE_FLAGS = 
	RUNTIME_ID=win-x64
	PATH_TAIL=$(BIN_PATH)/$(RUNTIME_ID)/publish
    EXECUTABLE=$(APP_DIR)/$(PATH_TAIL)/ConsoleInterface.exe
endif

EXTERNAL_LIBS_DIR := ./ExportLibrary/externalLibs
APP_DIR := ./ConsoleInterface
GRAPH_DIR := ./S21_graph
ALG_DIR := ./S21_graph_algorithms
TEST_DIR := ./TestSimpleNavigator
COVERAGE_DIR := coverage
BIN_PATH := bin/Debug/net8.0

all: s21_graph s21_graph_algorithms test run

build_libraries:
	$(MAKE) -C $(EXTERNAL_LIBS_DIR)

copy_libraries: build_libraries
ifeq ($(OS),Linux)
	mkdir -p $(TEST_DIR)/$(BIN_PATH)/$(RUNTIME_ID)
	mkdir -p $(APP_DIR)/$(PATH_TAIL)
	mkdir -p $(GRAPH_DIR)/$(PATH_TAIL)
	mkdir -p $(ALG_DIR)/$(PATH_TAIL)
	cp $(EXTERNAL_LIBS_DIR)/*.so $(TEST_DIR)/$(BIN_PATH)/$(RUNTIME_ID)/
	cp $(EXTERNAL_LIBS_DIR)/*.so $(APP_DIR)/$(PATH_TAIL)/
	cp $(EXTERNAL_LIBS_DIR)/*.so $(GRAPH_DIR)/$(PATH_TAIL)/
	cp $(EXTERNAL_LIBS_DIR)/*.so $(ALG_DIR)/$(PATH_TAIL)/
else
	xcopy /Y /Q "$(EXTERNAL_LIBS_DIR)\*.dll" "$(TEST_DIR)\$(BIN_PATH)\$(RUNTIME_ID)\"
	xcopy /Y /Q "$(EXTERNAL_LIBS_DIR)\*.dll" "$(APP_DIR)\$(PATH_TAIL)\"
	xcopy /Y /Q "$(EXTERNAL_LIBS_DIR)\*.dll" "$(GRAPH_DIR)\$(PATH_TAIL)\"
	xcopy /Y /Q "$(EXTERNAL_LIBS_DIR)\*.dll" "$(ALG_DIR)\$(PATH_TAIL)\"
endif

s21_graph: build_libraries copy_libraries
	dotnet build $(GRAPH_DIR)

s21_graph_algorithms: build_libraries copy_libraries
	dotnet build $(ALG_DIR)

build_csharp: build_libraries copy_libraries
	dotnet build $(APP_DIR)

run: build_libraries copy_libraries
	dotnet publish -r $(RUNTIME_ID) --self-contained -o $(APP_DIR)/$(PATH_TAIL)/
	$(EXECUTABLE)

test: build_libraries copy_libraries
	dotnet test $(TEST_DIR) --runtime $(RUNTIME_ID) /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
	reportgenerator -reports:./**/coverage.opencover.xml -targetdir:$(COVERAGE_DIR)

clean: clean_cplusplus clean_csharp	
ifeq ($(OS),Linux)
	rm -rf $(COVERAGE_DIR)
else
	if exist "$(COVERAGE_DIR)" rmdir /S /Q "$(COVERAGE_DIR)"
endif

clean_cplusplus:
ifeq ($(OS),Linux)
	rm -f $(EXTERNAL_LIBS_DIR)/*.so
	rm -f $(APP_DIR)/$(PATH_TAIL)/*.so
	rm -f $(TEST_DIR)/$(BIN_PATH)/$(RUNTIME_ID)/*.so
	rm -f $(GRAPH_DIR)/$(PATH_TAIL)/*.so
	rm -f $(ALG_DIR)/$(PATH_TAIL)/*.so
else
	if exist "$(EXTERNAL_LIBS_DIR)\*.dll"  del /Q "$(EXTERNAL_LIBS_DIR)\*.dll" 
	if exist "$(APP_DIR)\$(PATH_TAIL)\*.dll" del /Q "$(APP_DIR)\$(PATH_TAIL)\*.dll"
	if exist "$(TEST_DIR)\$(BIN_PATH)\$(RUNTIME_ID)\*.dll" del /Q "$(TEST_DIR)\$(BIN_PATH)\$(RUNTIME_ID)\*.dll"
	if exist "$(GRAPH_DIR)\$(PATH_TAIL)\*.dll" del /Q "$(GRAPH_DIR)\$(PATH_TAIL)\*.dll"
	if exist "$(ALG_DIR)\$(PATH_TAIL)\*.dll" del /Q "$(ALG_DIR)\$(PATH_TAIL)\*.dll"
endif

clean_csharp:
# cd TestSimpleNavigator
# dotnet remove package xunit.analyzers
# dotnet add package xunit.analyzers --version 1.4.0
# cd ..
	dotnet clean
ifeq ($(OS),Linux)
	rm -rf ./ExportLibrary/bin
	rm -rf ./Helpers/bin
	rm -rf $(APP_DIR)/bin
	rm -rf $(TEST_DIR)/bin
	rm -rf $(GRAPH_DIR)/bin
	rm -rf $(ALG_DIR)/bin
	rm -rf ./ExportLibrary/obj
	rm -rf ./Helpers/obj
	rm -rf $(APP_DIR)/obj
	rm -rf $(TEST_DIR)/obj
	rm -rf $(GRAPH_DIR)/obj
	rm -rf $(ALG_DIR)/obj
else
	if exist "./ExportLibrary/bin" rmdir /S /Q "./ExportLibrary/bin"
	if exist "./Helpers/bin" rmdir /S /Q "./Helpers/bin"
	if exist "$(APP_DIR)/bin" rmdir /S /Q "$(APP_DIR)/bin"
	if exist "$(TEST_DIR)/bin" rmdir /S /Q "$(TEST_DIR)/bin"
	if exist "$(GRAPH_DIR)/bin" rmdir /S /Q "$(GRAPH_DIR)/bin"
	if exist "$(ALG_DIR)/bin" rmdir /S /Q "$(ALG_DIR)/bin"
	if exist "./ExportLibrary/obj" rmdir /S /Q "./ExportLibrary/obj"
	if exist "./Helpers/obj" rmdir /S /Q "./Helpers/obj"
	if exist "$(APP_DIR)/obj" rmdir /S /Q "$(APP_DIR)/obj"
	if exist "$(TEST_DIR)/obj" rmdir /S /Q "$(TEST_DIR)/obj"
	if exist "$(GRAPH_DIR)/obj" rmdir /S /Q "$(GRAPH_DIR)/obj"
	if exist "$(ALG_DIR)/obj" rmdir /S /Q "$(ALG_DIR)/obj"
endif

format:
	cp ../materials/linters/.clang-format ./
	clang-format -i $(EXTERNAL_LIBS_DIR)/*.cpp\
					$(EXTERNAL_LIBS_DIR)/*.h\
					$(APP_DIR)/*.cs\
					$(GRAPH_DIR)/*.cs\
					$(ALG_DIR)/*.cs\
					$(TEST_DIR)/*.cs \
					./ExportLibrary/*.cs \
					./Helpers/*.cs \
					./ChooseParameters/*.cs \
					./Helpers/Containers/*.cs
	rm .clang-format

check-format:
	cp ../materials/linters/.clang-format ./
	clang-format -n $(EXTERNAL_LIBS_DIR)/*.cpp\
					$(EXTERNAL_LIBS_DIR)/*.h\
					$(APP_DIR)/*.cs\
					$(GRAPH_DIR)/*.cs\
					$(ALG_DIR)/*.cs\
					$(TEST_DIR)/*.cs \
					./ExportLibrary/*.cs \
					./Helpers/*.cs \
					./ChooseParameters/*.cs \
					./Helpers/Containers/*.cs
	rm .clang-format


.PHONY: all build_libraries s21_graph s21_graph_algorithms build_csharp run test clean clean_cplusplus clean_csharp format check-format