# import required modules
import asyncio
import websockets
import json
from pymongo import MongoClient
 
# Connect to MongoDB
client = MongoClient("192.168.0.36:27017")
db = client["player_position_realtime"]
collection = db["player_position"]
 
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

        # Save the position data to MongoDB
        query = { "player_id": player_id }
        new_values = { "$set": {
            "position_x": position_x,
            "position_y": position_y,
            "position_z": position_z,
            "rotation_x": rotation_x,
            "rotation_y": rotation_y,
            "rotation_z": rotation_z
        }}
        collection.update_one(query, new_values, upsert=True)

# Connect to the WebSocket and register the callback function
async def main():
    async with websockets.serve(on_message, '0.0.0.0', 8765):
        await asyncio.Future()  # run forever
asyncio.run(main())
