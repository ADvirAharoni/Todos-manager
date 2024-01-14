import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: "http://localhost:5224"
});

axiosInstance.interceptors.response.use(
  function (response) {
    return response;
  },
  function (error) {
    console.log(error);
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
    const result = await axiosInstance.get("/todos")    
    return result.data;
  },

  addTask: async(name)=>{
    const result = await axiosInstance.post(`/todos`, {"Name": name, "IsComplete":false});
    return result.data;
  },

  setCompleted: async(id, isComplete)=>{
    const result = await axiosInstance.put(`/todos/${id}`, {"IsComplete":isComplete});
    return result.data;
  },

  deleteTask:async(id)=>{
    await axiosInstance.delete(`/todos/${id}`);
  }
};
