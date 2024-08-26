CXX = g++

# Имя библиотек
QUEUE_LIB_NAME = s21_queue
STACK_LIB_NAME = s21_stack

# Исходные файлы
QUEUE_SRC_FILES = s21queue_wrapper.cpp
STACK_SRC_FILES = s21stack_wrapper.cpp

# Опции компиляции
CXXFLAGS = -std=c++17 -O2 -fPIC -m64

# Опции линковки для shared library
LDFLAGS = -shared

# Определение платформы и разрядности
OS := $(shell uname -s)
ifeq ($(OS), Linux)
    QUEUE_TARGET = lib$(QUEUE_LIB_NAME).so
    STACK_TARGET = lib$(STACK_LIB_NAME).so
    CXXFLAGS += -D LINUX
else
    QUEUE_TARGET = $(QUEUE_LIB_NAME).dll
    STACK_TARGET = $(STACK_LIB_NAME).dll
    CXXFLAGS += -D WINDOWS
endif

# Правило по умолчанию
all: $(QUEUE_TARGET) $(STACK_TARGET)

# Правила для создания целевых библиотек
$(QUEUE_TARGET):
	$(CXX) $(LDFLAGS) $(CXXFLAGS) $(QUEUE_SRC_FILES) -o $(QUEUE_TARGET)

$(STACK_TARGET): 
	$(CXX) $(LDFLAGS) $(CXXFLAGS) $(STACK_SRC_FILES) -o $(STACK_TARGET)

# Правило очистки
clean:
	rm -f *.o $(QUEUE_TARGET) $(STACK_TARGET)
