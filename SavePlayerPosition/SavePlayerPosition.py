# import required modules
import asyncio
import websockets
import json
import sqlite3
from sqlite3 import Error as Err
 
# explicit function to connect  database
# that resides in the memory
def SQLite_connection():
 
    try:         
        conn = sqlite3.connect('player_position.db')

        conn.execute("""
            CREATE TABLE IF NOT EXISTS player_position (
                player_id INTEGER NOT NULL,
                position_x NUMERIC NOT NULL,
                position_y NUMERIC NOT NULL,
                position_z NUMERIC NOT NULL,
                rotation_x NUMERIC NOT NULL,
                rotation_y NUMERIC NOT NULL,
                rotation_z NUMERIC NOT NULL
            );
        """)
 
    # if any interruption or error occurs
    except Err: print(Err)
 
    # terminate the connection   
    finally: conn.close()
    
def SavePosition(id, p_x, p_y, p_z, r_x, r_y, r_z):
    try:         
        conn = sqlite3.connect('player_position.db')

        conn.execute(f"""
        INSERT INTO player_position  (player_id, position_x , position_y, position_z, rotation_x, rotation_y, rotation_z) 
        VALUES({id}, {p_x}, {p_y}, {p_z}, {r_x}, {r_y}, {r_z}) 
        ON CONFLICT(player_id)
        DO UPDATE SET
        position_x = {p_x}, 
        position_y = {p_y}, 
        position_z = {p_z}, 
        rotation_x = {r_x}, 
        rotation_y = {r_y},
        rotation_z = {r_z};
        """)
        conn.commit();
 
    # if any interruption or error occurs
    except Err: print(Err)
 
    # terminate the connection   
    finally: conn.close()


async def on_message(websocket):
    async for message in websocket:
        # Parse the JSON message

        data = json.loads(message)

        # Extract the position data
        player_id = data['player_id']
        position_x = data['position_x']
        position_y = data['position_y']
        position_z = data['position_z']
        rotation_x = data['rotation_x']
        rotation_y = data['rotation_y']
        rotation_z = data['rotation_z']
        SavePosition(player_id, position_x, position_y, position_z, rotation_x, rotation_y, rotation_z)

# Connect to the WebSocket and register the callback function
start_server = websockets.serve(on_message, "localhost", 8765)

asyncio.get_event_loop().run_until_complete(start_server)
asyncio.get_event_loop().run_forever()

# function call       
SQLite_connection()
