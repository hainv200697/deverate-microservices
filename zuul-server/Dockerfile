FROM java:8 as build

WORKDIR /app

COPY . .

RUN chmod +x ./gradlew

RUN ./gradlew build

FROM openjdk:8-jre-alpine

WORKDIR /opt/

COPY --from=build /app/build/libs/deverate-0.0.1-SNAPSHOT.jar /opt/zuul.jar

EXPOSE 8760

ENTRYPOINT ["java", "-jar", "/opt/zuul.jar"]
