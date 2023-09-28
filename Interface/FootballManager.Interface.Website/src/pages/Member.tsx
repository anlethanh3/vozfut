import { useEffect, useCallback, useMemo } from "react";
import { Button, Col, Row, Table, Pagination, Alert, DropdownButton, Dropdown, OverlayTrigger, Tooltip, } from "react-bootstrap";
import moment from 'moment'
import AddMember from "../components/AddMember"
import UpdateMember from "../components/UpdateMember"
import Confirmation from "../components/Confirmation"
import SearchMember from "../components/SearchMember";
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { selectState, fetchAsync, MemberProps, SearchProps, onChangePageIndex, onChangePageSize, onShowLegend, onShowAdd, onShowDelete, onSelectedId, onShowUpdate, updateAsync, addAsync, deleteAsync, onSearchChanged, searchChangedAsync, } from '../slices/memberSlice';
import Legend from "../components/Legend";
import { FaEdit, FaTrash, FaPlus, FaFileUpload, FaBeer, FaStar } from "react-icons/fa";
import { debounce } from 'lodash'

export default function Member() {
    const sizes = [10, 50, 100]
    const state = useAppSelector(selectState)
    const dispatch = useAppDispatch()
    moment.defaultFormat = 'YYYY-MM-DD HH:mm:ss'
    const fetchData = async () => {
        dispatch(fetchAsync({ name: state.search.name, pageIndex: state.pageIndex, pageSize: state.pageSize })).unwrap()
            .then(value => {
            })
            .catch(ex => {
            })
    }

    const onSubmitAdd = async (member: MemberProps) => {
        dispatch(onShowAdd(false))
        dispatch(addAsync(member)).unwrap()
            .then(response => fetchData())
            .catch(error => {
                dispatch({ type: "failure", error: "Access denied! Requested Add!" })
            })
    }
    const onSubmitUpdate = (member: MemberProps) => {
        dispatch(onShowUpdate(false))
        dispatch(updateAsync(member)).unwrap()
            .then(response => fetchData())
            .catch(error => {
                console.log(error)
            })
    }
    const onDelete = async () => {
        dispatch(onShowDelete(false))
        dispatch(deleteAsync(state.selectedId)).unwrap()
            .then(response => fetchData())
            .catch(error => {
            })

    }

    const onSearchEvent = () => {
        fetchData()
    }
    const searchRequest = useCallback((search: SearchProps) =>
        dispatch(searchChangedAsync({ name: search.name, pageIndex: state.pageIndex, pageSize: state.pageSize }))
            .unwrap()
            .then(value => dispatch(onSearchChanged(search.name)))
            .catch(ex => {
            }), [])

    const debouncedSendRequest = useMemo(() => debounce(searchRequest, 500), [searchRequest]);
    const onSearchChangedEvent = async (search: SearchProps) => {
        debouncedSendRequest(search)
    }

    function ChampionStars(props: { championCount: number }): JSX.Element {
        let { championCount } = props
        let items: JSX.Element[] = []
        for (let index = 0; index < championCount; index++) {
            items.push(<FaStar />)
        }
        return (<>{items}</>)
    }

    useEffect(() => {
        fetchData()
        console.log('effect', state)

        return function cleanup() {
            console.log('unmount', state)
        }

    }, [state.pageIndex, state.pageSize,])
    return (
        <>
            <h1>Members</h1>
            {
                state.isLoading &&
                <Alert color='primary'>Loading...</Alert>
            }
            {
                state.error &&
                <Alert show={state.error !== undefined} variant="danger" onClose={() => dispatch({ type: "failure", error: undefined })} dismissible>
                    {state.error}
                </Alert>
            }
            {
                state.isShowLegend &&
                <Legend show={state.isShowLegend} onClose={() => dispatch(onShowLegend(false))} />
            }
            {
                state.isShowAdd &&
                <AddMember show={state.isShowAdd} onSubmit={(member) => onSubmitAdd(member)} onClose={() => dispatch(onShowAdd(false))} />
            }
            {
                state.isShowUpdate &&
                <UpdateMember initData={state.data.find(x => x.id === state.selectedId)} show={state.isShowUpdate} onSubmit={(member) => onSubmitUpdate(member)} onClose={() => dispatch(onShowUpdate(false))} />
            }
            {
                state.isShowDelete &&
                <Confirmation
                    show={state.isShowDelete}
                    title={"Confirmation"}
                    content={"Are you sure you want to permanently delete this member?"}
                    onSubmit={onDelete}
                    onClose={() => dispatch(onShowDelete(false))} />
            }

            <SearchMember onSearchChanged={onSearchChangedEvent} onSubmit={onSearchEvent} />

            <Row className="my-2">
                <Col className="d-flex justify-content-end">
                    <OverlayTrigger overlay={<Tooltip>Add a Member</Tooltip>}>
                        <Button variant="primary" onClick={() => dispatch(onShowAdd(true))}>  <FaPlus /> </Button>
                    </OverlayTrigger>
                    <OverlayTrigger overlay={<Tooltip>Import CSV</Tooltip>}>
                        <Button variant="secondary" className="mx-2"><FaFileUpload /></Button>
                    </OverlayTrigger>
                    <OverlayTrigger overlay={<Tooltip>Show legend</Tooltip>}>
                        <Button variant="success" onClick={() => dispatch(onShowLegend(true))}><FaBeer /></Button>
                    </OverlayTrigger>
                </Col>
            </Row>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>No</th>
                        <th>Name</th>
                        <th>Nick Name</th>
                        <th>Description</th>
                        <th>Champion Count</th>
                        <th>Elo</th>
                        <th>Speed</th>
                        <th>Stamina</th>
                        <th>Finishing</th>
                        <th>Passing</th>
                        <th>Skill</th>
                        <th>Modified Date</th>
                        <th className="col-sm-2">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        state.data && state.data.map((value, index) =>
                            <tr key={`key-${value.id}`}>
                                <td>{index + 1}</td>
                                <td>{value.realName}</td>
                                <td>{value.name}</td>
                                <td>{value.description}</td>
                                <td><ChampionStars championCount={value?.championCount ?? 0} /></td>
                                <td className="col-sm-1">+{value.elo}</td>
                                <td className="col-sm-1">{value.speed}</td>
                                <td className="col-sm-1">{value.stamina}</td>
                                <td className="col-sm-1">{value.finishing}</td>
                                <td className="col-sm-1">{value.passing}</td>
                                <td className="col-sm-1">{value.skill}</td>
                                <td>{value.modifiedDate && moment(value.modifiedDate).format()}</td>
                                <td>
                                    <OverlayTrigger overlay={<Tooltip>Edit</Tooltip>}>
                                        <Button variant="warning" onClick={() => {
                                            dispatch(onSelectedId(value.id))
                                            dispatch(onShowUpdate(true))
                                        }}><FaEdit /></Button>
                                    </OverlayTrigger>
                                    <OverlayTrigger overlay={<Tooltip>Delete</Tooltip>}>
                                        <Button variant="danger" className="mx-2" onClick={() => {
                                            dispatch(onSelectedId(value.id))
                                            dispatch(onShowDelete(true))
                                        }}><FaTrash /></Button>
                                    </OverlayTrigger>
                                </td>
                            </tr>
                        )
                    }
                </tbody>
            </Table>
            {
                state.totalPage &&
                <>
                    <Row>
                        <Col>
                            <DropdownButton className="d-inline" size="lg" variant="secondary" id="dropdown-basic-button" title={state.pageSize}>
                                {
                                    sizes.map(element =>
                                        <Dropdown.Item key={`ditem-${element}`} onClick={() => dispatch(onChangePageSize(element))}>{element}</Dropdown.Item>
                                    )
                                }
                            </DropdownButton> <p className="d-inline">Records Page {state.pageIndex + 1} of {state.totalPage}</p></Col>
                        <Col>
                            <Pagination className="d-flex justify-content-end">
                                {
                                    Array.from(Array(state.totalPage).keys()).map(element => {
                                        var pageNumber = element + 1
                                        return <Pagination.Item
                                            onClick={() => dispatch(onChangePageIndex(pageNumber - 1))}
                                            key={`pitem-${element}`}
                                            active={state.pageIndex === pageNumber - 1}>{pageNumber}</Pagination.Item>
                                    })
                                }
                            </Pagination>
                        </Col>
                    </Row>
                </>
            }

        </>
    )
}